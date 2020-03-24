using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using EmployeeWebApi.DependencyInjection;

namespace EmployeeWebApi
{
    public static class IoC
    {
        private static IUnityContainer container;

        public static void Initialize()
        {
            container = new UnityContainer();
            DependencyResolver resolver = new DependencyResolver();
            resolver.Initialize(container);
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        public static T Resolve<T>(string name)
        {
            return container.Resolve<T>(name);
        }

        public static object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        public static void RegisterInstance<T>(T obj)
        {
            container.RegisterInstance<T>(obj);
        }

        public static void RegisterInstance<T>(string name, T object2Register)
        {
            container.RegisterInstance<T>(name, object2Register);
        }

        public static bool IsRegistered<T>(string name)
        {
            return container.IsRegistered<T>(name);
        }

        public static bool IsRegistered<T>()
        {
            return container.IsRegistered<T>();
        }
    }
}