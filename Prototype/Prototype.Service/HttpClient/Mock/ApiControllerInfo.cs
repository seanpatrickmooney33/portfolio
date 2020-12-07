using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prototype.Service.HttpClient.Mock
{
    public class ApiControllerInfo : IComparable<ApiControllerInfo>, IComparer<ApiControllerInfo>, IEquatable<ApiControllerInfo>
    {
        public Type ApiControllerType { get; set; }
        public RouteAttribute RoutePrefixAttribute { get; set; }
        public List<RouteInfo> RouteInfos { get; set; }

        public String RoutePrefix { get; set; }

        public ApiControllerInfo(Type apiControllerType)
        {
            this.ApiControllerType = apiControllerType;
            this.RoutePrefixAttribute = this.ApiControllerType.GetCustomAttribute<RouteAttribute>();
            this.RoutePrefix = this.RoutePrefixAttribute?.Template;
            this.RouteInfos = this.ApiControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                                              .Select(x => new RouteInfo(this, x))
                                                              .Where(x => x != null)
                                                              .ToList();

            RouteDefinitionValidator.Validate(this.RoutePrefix, "Route prefix");
        }

        public int Compare(ApiControllerInfo x, ApiControllerInfo y)
        {
            return x.CompareTo(y);
        }

        public int CompareTo(ApiControllerInfo other)
        {
            return this.ApiControllerType.FullName.CompareTo(other.ApiControllerType.FullName);
        }

        public bool Equals(ApiControllerInfo other)
        {
            return (this.CompareTo(other) == 0);
        }

        public override int GetHashCode()
        {
            return this.ApiControllerType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is ApiControllerInfo) ? this.Equals(obj as ApiControllerInfo) : false;
        }
    }

}
