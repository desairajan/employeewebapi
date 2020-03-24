using EmployeeWebApi.BusinessImplementation;
using EmployeeWebApi.BusinessInterfaces;
using EmployeeWebApi.DAL.Implementation;
using EmployeeWebApi.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using EmployeeWebApi.BusinessImplementation;
using EmployeeWebApi.BusinessInterfaces;

namespace EmployeeWebApi.DependencyInjection
{
    public class DependencyResolver
    {
        public void Initialize(IUnityContainer container)
        {
            //Business
            container.RegisterType<IApiCacheManager, ApiCacheManager>();
            container.RegisterType<IApiRequestIdCacheManager, ApiRequestIdCacheManager>();
            container.RegisterType<IEmployeeApiValidationManager, EmployeeApiValidationManager>();
            container.RegisterType<IEmployeeManager, EmployeeManager>();
            container.RegisterType<IValuesManager, ValuesManager>();
            container.RegisterType<IApiLogManager, ApiLogManager>();


            //DAL
            container.RegisterType<ISqlDatabase, SqlDatabase>();
            container.RegisterType<ISysApiDao, SysApiDao>();
            container.RegisterType<IEmployeeDao, EmployeeDao>();
            container.RegisterType<IApiLogDao, ApiLogDao>();
        }
    }
}