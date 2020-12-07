using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Prototype.Service.HttpClient.Mock
{
    public class MockSOSHttpClient<T> : ISOSHttpClient<T>
    {
        //public List<KeyValuePair<String, ApiControllerInfo>> ControllerList { get; set; }
        public List<RouteInfo> RouteInfoList { get; set; }
        public List<RouteInfo> ParameterizedRouteInfoList { get; set; }
        public List<RouteInfo> NonParameterizedRouteInfoList { get; set; }

        private MockSOSHttpClientFactory Factory { get; set; }
        private String AssemblyName { get; set; }

        public MockSOSHttpClient(MockSOSHttpClientFactory factory, String assemblyName)
        {
            this.Factory = factory;
            this.AssemblyName = assemblyName;
            this.RouteInfoList = Mock(this.AssemblyName);
            this.ParameterizedRouteInfoList = this.RouteInfoList.Where(x => x.IsParameterized == true).ToList();
            this.NonParameterizedRouteInfoList = this.RouteInfoList.Where(x => x.IsParameterized == false).ToList();
        }

        public T Create(T entity, String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpPostAttribute), typeof(T));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);

            dynamic result = routeInfo.MethodInfo.Invoke(classInstance, new Object[] { entity });
            return HandleSingleResult(result);
        }

        public void Delete(String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpDeleteAttribute), typeof(ActionResult));
            Object classInstance = Activator.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = CreateParamArray(routeInfo, url);

            routeInfo.MethodInfo.Invoke(classInstance, paramArray);
        }

        public void Dispose()
        {
            //  TODO:  Need to clean up if necessary.
            //throw new NotImplementedException();
        }

        public IEnumerable<T> GetMultiple(String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpGetAttribute), typeof(IEnumerable<T>));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = CreateParamArray(routeInfo, url);

            dynamic result = routeInfo.MethodInfo.Invoke(classInstance, paramArray);
            return HandleMultipleResult(result);
            //dynamic content = result.Content;

            //return ( IEnumerable<T> )content;
        }

        public T GetSingle(String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpGetAttribute), typeof(T));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = CreateParamArray(routeInfo, url);

            dynamic result = routeInfo.MethodInfo.Invoke(classInstance, paramArray);
            return HandleSingleResult(result);
        }

        public void GetVoid(String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpGetAttribute), typeof(ActionResult));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = CreateParamArray(routeInfo, url);

            routeInfo.MethodInfo.Invoke(classInstance, paramArray);
        }

        public void Remove(T entity, String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpPostAttribute), typeof(ActionResult));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = CreateParamArray(routeInfo, url);

            routeInfo.MethodInfo.Invoke(classInstance, (entity != null) ? new Object[] { entity } : paramArray);
        }

        public void Update(T entity, String url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpPutAttribute), typeof(ActionResult));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = CreateParamArray(routeInfo, url);

            routeInfo.MethodInfo.Invoke(classInstance, (entity != null) ? new Object[] { entity } : paramArray);
        }


        private List<RouteInfo> Mock(String assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            List<Type> types = assembly.GetTypes().Where(x => (x.IsSubclassOf(typeof(ControllerBase)) == true) && (x.IsPublic == true) && (x.IsAbstract == false))
                                                     .ToList();

            List<RouteInfo> routeInfos = types.Select(x => new ApiControllerInfo(x))
                                              .SelectMany(x => x.RouteInfos)
                                              .OrderByDescending(x => x.URL.Length)
                                              .ToList();

            return routeInfos;
        }

        private RouteInfo GetRouteInfo(String url, Type verbType, Type returnType)
        {
            RouteInfo routeInfo = null;
            String[] urlFields = url.Split('?');

            //routeInfo = (urlFields.Count() > 1) ? GetRouteInfoWithParameter(urlFields, verbType, returnType, this.ParameterizedRouteInfoList)
            //                                      : GetRouteInfoWithURL(urlFields[0], verbType, returnType, this.NonParameterizedRouteInfoList);

            routeInfo = GetRouteInfoWithURL(urlFields[0], verbType, returnType, this.ParameterizedRouteInfoList);

            if ((routeInfo == null) && (urlFields.Count() > 1))
            {
                routeInfo = GetRouteInfoWithURL(urlFields[0], verbType, returnType, this.NonParameterizedRouteInfoList);
            }

            if (routeInfo == null)
            {
                throw new InvalidOperationException(String.Format("Cannot find resource for [{0}]", url));
            }

            return routeInfo;
        }

        private RouteInfo GetRouteInfoWithParameter(String[] urlFields, Type verbType, Type returnType, List<RouteInfo> routes)
        {
            List<RouteInfo> candidateRoutes = routes.Where(x => ((String.Compare(x.URL, urlFields[0], true) == 0) && (x.VerbType == verbType) && (x.ReturnType == returnType)))
                                                    .ToList();
            String[] paramFields = urlFields.Skip(1).ToArray();

            candidateRoutes = candidateRoutes.Where(x => x.MinParameter <= paramFields.Count())
                                             .ToList();

            foreach (String param in paramFields)
            {
                candidateRoutes = candidateRoutes.Where(x => x.ActionParams.Where(y => String.Compare(y.Key, param.Trim(), true) == 0)
                                                                           .Count() > 0)
                                                 .ToList();
            }

            if (candidateRoutes.Count() > 0)
            {
                throw new AmbiguousMatchException(String.Format("Found {0} match for {1}", candidateRoutes.Count(), urlFields.Aggregate((a, b) => a + b)));
            }

            return candidateRoutes.FirstOrDefault();
        }

        private RouteInfo GetRouteInfoWithURL(String url, Type verbType, Type returnType, List<RouteInfo> routes)
        {
            //  TODO:  Need to be handle these:
            //         [Route( "Index/{name}/foo/{id}/goo" )]
            //         [Route( "Index/{name}/foo/goo/{id}" )]


            List<RouteInfo> candidateRoutes = routes.Where(x => (url.StartsWith(x.URL, StringComparison.CurrentCultureIgnoreCase) == true) &&
                                                                (x.VerbType == verbType) &&
                                                                (x.ReturnType == returnType) &&
                                                                (url.Remove(0, x.ApiControllerInfo.RoutePrefix.Length).Length > 0) &&  // identify that this url
                                                                (url.Remove(0, x.ApiControllerInfo.RoutePrefix.Length)[0] == '/'))  // belongs to the controller
                                                    .OrderBy(x => x.URL.Length)
                                                    .ToList();

            if (candidateRoutes.Count() == 0)
            {
                throw new InvalidOperationException(String.Format("Cannot find resource for [{0}]", url));
            }

            List<ApiControllerInfo> candidateAPIController = candidateRoutes.Select(x => x.ApiControllerInfo)
                                                                            .Distinct()
                                                                            .ToList();

            if ((candidateAPIController.Count() == 0) || (url.Length <= candidateAPIController.First().RoutePrefix.Length + 2))
            {
                throw new InvalidOperationException(String.Format("Cannot find resource for [{0}]", url));
            }
            if (candidateAPIController.Count() > 1)
            {
                throw new AmbiguousMatchException(String.Format("Found {0} controller{1} for {2}", candidateAPIController.Count(), (candidateAPIController.Count() > 1) ? "s" : "", url));
            }

            String paramString = url.Remove(0, candidateAPIController.First().RoutePrefix.Length + 1);
            String[] paramFields = paramString.Split('/');

            candidateRoutes = candidateRoutes.Where(x => String.Compare(x.ActionName, paramFields[0], true) == 0)
                                             .ToList();

            paramFields = paramFields.Skip(1).ToArray();
            candidateRoutes = (paramFields.Length > 0) ? candidateRoutes.Where(x => x.ActionParams.Count() <= paramFields.Length)
                                                                         ?.OrderBy(x => x.ActionParams.Count())
                                                                          .ToList()
                                                         : candidateRoutes;

            return (candidateRoutes != null) ? candidateRoutes.FirstOrDefault() : null;
        }

        private Object[] CreateParamArray(RouteInfo routeInfo, String url)
        {
            List<Object> paramList = new List<Object>();
            String tempURL = url.Remove(0, routeInfo.URL.Length);
            String[] paramFields = null;
            String[] nvp = null;
            List<String> names = null;


            if (tempURL.Length > 0)
            {
                if (tempURL.StartsWith("?") == true)
                {
                    names = routeInfo.ActionParams.Select(x => x.Key).ToList();
                    paramFields = tempURL.Remove(0, 1).Split('&');
                    foreach (String paramField in paramFields)
                    {
                        nvp = paramField.Split('=');
                        if (names.Contains(nvp[0]) == false)
                        {
                            throw new ArgumentException(String.Format("No such parameter [{0}] for action [{1}] in controller [{2}]", nvp[0], routeInfo.ActionName, routeInfo.ApiControllerInfo.ApiControllerType.Name));
                        }
                        paramList.Add(TypeDescriptor.GetConverter(routeInfo.ActionParams.First(x => (String.Compare(x.Key, nvp[0], true) == 0)).Value.ParameterType).ConvertFromString((nvp.Length == 1) ? String.Empty : nvp[1]));
                    }
                }
                else
                {
                    paramFields = (tempURL.StartsWith("/") ? tempURL.Remove(0, 1) : tempURL).Split('/');
                    for (Int32 index = 0; index < paramFields.Length; index++)
                    {
                        paramList.Add(TypeDescriptor.GetConverter(routeInfo.ActionParams[index].Value.ParameterType).ConvertFromString(paramFields[index]));
                    }
                }
            }
            else
            {
                paramFields = new String[] { };
            }

            return paramList.ToArray();
        }

        private IEnumerable<T> HandleMultipleResult(Object result)
        {
            IEnumerable<T> content = ((result is IEnumerable<T>)) ? (IEnumerable<T>)result : null;
            if (content == null)
            {
                throw new Exception(String.Format("Unhandled result [{0}]", result.GetType()));
            }

            return content;
        }

        private T HandleSingleResult(Object result)
        {
            T response;

            if ((result is T) )
            {
                response = (T)result;
            }
            else
            {
                throw new Exception(String.Format("Unhandled result [{0}]", result.GetType()));
            }

            return response;
        }

        public IEnumerable<T> GetMultiple<P>(P param, string url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpPostAttribute), typeof(T));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = new object[] { param };

            dynamic result = routeInfo.MethodInfo.Invoke(classInstance, paramArray);
            return HandleMultipleResult(result);
        }

        public T GetSingle<P>(P param, string url)
        {
            RouteInfo routeInfo = GetRouteInfo(url, typeof(HttpPostAttribute), typeof(T));
            Object classInstance = this.Factory.CreateInstance(routeInfo.ApiControllerInfo.ApiControllerType);
            Object[] paramArray = new object[] { param };

            dynamic result = routeInfo.MethodInfo.Invoke(classInstance, paramArray);
            return HandleSingleResult(result);
        }
    }

}
