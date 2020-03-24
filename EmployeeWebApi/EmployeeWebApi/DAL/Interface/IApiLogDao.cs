using EmployeeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.DAL.Interface
{
    public interface IApiLogDao
    {
        long InsertIntoApiLog(ApiLogViewModel model);
        void UpdateApiLogWithResponseAndTime(long apiLogId, string response, long timeInMs);
    }
}
