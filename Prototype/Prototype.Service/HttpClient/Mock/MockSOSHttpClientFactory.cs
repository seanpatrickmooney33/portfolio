
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Prototype.Service.HttpClient.Mock
{
    public class MockSOSHttpClientFactory : IHttpClientFactory
    {
        private String WebAPIAssemblyName { get; set; }
        private Dictionary<Type, Object> Parameters { get; set; } = new Dictionary<Type, Object>();

        public MockSOSHttpClientFactory(String webAPIAssemblyName)
        {
            this.WebAPIAssemblyName = webAPIAssemblyName;
        }

        public void Register<T>(Object instance)
        {
            Type interfaceClass = typeof(T);
            if (interfaceClass.IsInterface != true) { throw new ArgumentException(String.Format("{0} is not an interface class.", interfaceClass.ToString())); }
            if ((instance.GetType().GetInterfaces().Count() == 0) ||
                 (instance.GetType().GetInterfaces().Where(x => x == interfaceClass).Count() == 0)) { throw new ArgumentException(String.Format("Instance parameter provided is not an implemenation of {0}.", interfaceClass.ToString())); }
            this.Parameters[interfaceClass] = instance;
        }

        public Object GetInstance(Type interfaceClass)
        {
            if (interfaceClass.IsInterface != true) { throw new ArgumentException(String.Format("{0} is not an interface class.", interfaceClass.ToString())); }
            return this.Parameters[interfaceClass];
        }

        public ISOSHttpClient<T> CreateClient<T>()
        {
            return new MockSOSHttpClient<T>(this, this.WebAPIAssemblyName);
        }

        public Object CreateInstance(Type controllerType)
        {
            //  TODO:  Handle default parameter values.

            ConstructorInfo[] constructors = controllerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            if (constructors.Count() != 1) { throw new InvalidOperationException(String.Format("{0} constructor defined for {1}.", (constructors.Count() == 0) ? "No" : "Multiple", controllerType.ToString())); }
            ParameterInfo[] parameterInfo = constructors[0].GetParameters();
            List<Object> parameters = new List<Object>();

            foreach (ParameterInfo info in parameterInfo)
            {
                if (this.Parameters.ContainsKey(info.ParameterType) == false) { throw new ArgumentException(String.Format("No definition for {0}.", info.ParameterType.ToString())); }
                parameters.Add(this.Parameters[info.ParameterType]);
            }

            return Activator.CreateInstance(controllerType, parameters.ToArray());
        }
    }

}
