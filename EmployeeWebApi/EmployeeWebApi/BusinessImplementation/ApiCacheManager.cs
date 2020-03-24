using EmployeeWebApi.BusinessInterfaces;
using EmployeeWebApi.DAL.Interface;
using EmployeeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.BusinessImplementation
{
    public class ApiCacheManager : IApiCacheManager
    {
        private ISysApiDao _apiDao;

        public static Dictionary<string, SysApi> apiDic;
        public static Dictionary<Tuple<int, string>, SysApiAccessToken> apiAccessDic;
        
        public ApiCacheManager(ISysApiDao apiDao)
        {
            _apiDao = apiDao;
        }

        public void CacheApi()
        {
            if (apiDic == null)
                apiDic = _apiDao.GetAllSysApi().ToDictionary(i => i.Code);
        }

        public void CacheApiAccessToken()
        {
            if (apiAccessDic == null)
                apiAccessDic = _apiDao.GetApiAccessToken().ToDictionary(i => new Tuple<int, string>(i.SysApiId, i.Token));
        }
    }
}