using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prototype.Service.HttpClient.Mock
{
    public class RouteInfo : IComparable<RouteInfo>, IComparer<RouteInfo>, IEquatable<RouteInfo>
    {
        public ApiControllerInfo ApiControllerInfo { get; set; }
        public CustomAttributeData RouteAttribute { get; set; }
        public Type VerbType { get; set; }
        public Type ReturnType { get; set; }
        public MethodInfo MethodInfo { get; set; }

        public Boolean IsParameterized { get; set; }

        public String ActionName { get; set; }
        public String URL { get; set; }
        public UInt32 MinParameter { get; set; }

        public List<KeyValuePair<String, ParameterInfo>> ActionParams { get; set; }

        private Int32 HashCode { get; set; }

        public RouteInfo(ApiControllerInfo apiControllerInfo, MethodInfo methodInfo)
        {
            //  TODO: Future enhancement need to handle the following route format:
            //        [Route( "Index/{name}/foo/{id}/goo" )]
            //        [Route( "Index/{name}/foo/goo/{id}" )]

            this.ApiControllerInfo = apiControllerInfo;
            this.MethodInfo = methodInfo;
            this.ReturnType = this.MethodInfo.ReturnType;
            this.RouteAttribute = this.MethodInfo.CustomAttributes.Where(x => x.AttributeType == typeof(RouteAttribute))
                                                                     .FirstOrDefault();
            this.IsParameterized = (this.RouteAttribute == null) ? true : false;
            this.VerbType = this.MethodInfo.CustomAttributes.Where(z => z.AttributeType != typeof(RouteAttribute))
                                                                     .FirstOrDefault()?.AttributeType ?? typeof(HttpGetAttribute);

            String route = this.RouteAttribute?.ConstructorArguments.FirstOrDefault().Value as String;
            String[] routeFields = (String.IsNullOrWhiteSpace(route) == false) ? route.Split('/') : null;
            ParameterInfo[] parameterInfos = this.MethodInfo.GetParameters();

            this.ActionName = (routeFields != null) ? routeFields[0] : this.MethodInfo.Name;
            this.URL = String.Format("{0}/{1}", this.ApiControllerInfo.RoutePrefix, this.ActionName);
            this.ActionParams = ((routeFields != null) && (routeFields.Count() > 1)) ? routeFields.Skip(1).Where(x => ((x.Contains("{") == true) && (x.Contains("}") == true)))
                                                                                                                  .Select((x, index) => CreateActionParam(x.Replace("{", String.Empty).Replace("}", String.Empty).Replace("?", String.Empty), parameterInfos[index]))
                                                                                                                  .ToList()
                                                                                           : parameterInfos?.Select(x => new KeyValuePair<String, ParameterInfo>(x.Name, x)).ToList() ?? new List<KeyValuePair<String, ParameterInfo>>();
            this.MinParameter = GetMinParameter(this.ActionParams);
            this.HashCode = String.Format("{0}_{1}_{2}", this.URL, this.IsParameterized, (this.ActionParams.Count() > 0) ? this.ActionParams.Select(x => x.Key).Aggregate((a, b) => String.Format("{0}_{1}", a, b)) : String.Empty).GetHashCode();

            RouteDefinitionValidator.Validate(this.URL, "Route");
        }

        public int Compare(RouteInfo x, RouteInfo y)
        {
            return x.CompareTo(y);
        }

        public int CompareTo(RouteInfo other)
        {
            return this.HashCode.CompareTo(other.HashCode);
        }

        public bool Equals(RouteInfo other)
        {
            return (this.CompareTo(other) == 0);
        }

        public override int GetHashCode()
        {
            return this.HashCode;
        }

        public override bool Equals(object obj)
        {
            return (obj is RouteInfo) ? this.Equals(obj as RouteInfo) : false;
        }

        private KeyValuePair<String, ParameterInfo> CreateActionParam(String paramName, ParameterInfo parameterInfo)
        {
            String[] paramNameFields = paramName.Split('=');
            if (String.Compare(paramNameFields[0], parameterInfo.Name, true) != 0) { throw new ArgumentException("Parameter name mismatch"); }
            return new KeyValuePair<String, ParameterInfo>(paramName, parameterInfo);
        }

        private UInt32 GetMinParameter(List<KeyValuePair<String, ParameterInfo>> actionParams)
        {
            UInt32 minCount = 0;
            ParameterInfo param = null;
            for (Int32 index = actionParams.Count() - 1; index >= 0; index--)
            {
                param = actionParams[index].Value;
                if ((param == null) ||
                     (param.HasDefaultValue == true) ||
                     (param.IsOptional == true) ||
                     (param.ParameterType == typeof(Nullable<>)) ||
                     (param.ParameterType == typeof(String)))
                {
                    if (minCount != 0) { break; }
                    continue;
                }

                minCount++;
            }

            return minCount;
        }
    }

}
