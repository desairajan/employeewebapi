using EmployeeWebApi.BusinessInterfaces;
using EmployeeWebApi.DAL.Interface;
using EmployeeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.BusinessImplementation
{
    public class ApiLogManager: IApiLogManager
    {
        private IApiLogDao _apiLogDao;
        public ApiLogManager(IApiLogDao apiLogDao)
        {
            _apiLogDao = apiLogDao;
        }
        public long InsertIntoApiLog(ApiLogViewModel model)
        {
            return _apiLogDao.InsertIntoApiLog(model);
        }

        public void UpdateApiLogWithResponseAndTime(long apiLogId, string response, long timeInMs)
        {
            _apiLogDao.UpdateApiLogWithResponseAndTime(apiLogId, response, timeInMs);
        }
    }
}