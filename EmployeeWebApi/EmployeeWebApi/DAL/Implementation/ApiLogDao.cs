using EmployeeWebApi.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeWebApi.Models;
using System.Data.SqlClient;
using EmployeeWebApi;

namespace EmployeeWebApi.DAL.Implementation
{
    public class ApiLogDao : IApiLogDao
    {
        private ISqlDatabase _sqlDb;
        public ApiLogDao()
        {
            _sqlDb = IoC.Resolve<ISqlDatabase>();
        }
        public long InsertIntoApiLog(ApiLogViewModel model)
        {
            SqlCommand cmd = new SqlCommand("dbo.CoreApiLog_Insert");
            cmd.Parameters.AddWithValue("@ApiId", model.SysApiId);
            cmd.Parameters.AddWithValue("@RequestId", model.RequestId);
            cmd.Parameters.AddWithValue("@Token", model.Token);
            cmd.Parameters.AddWithValue("@Request", model.Request);
            var res = _sqlDb.ExecuteScalar(cmd);
            return Convert.ToInt64(res);

        }
        public void UpdateApiLogWithResponseAndTime(long apiLogId,string response,long timeInMs)
        {
            SqlCommand cmd = new SqlCommand("dbo.CoreApiLog_Update");
            cmd.Parameters.AddWithValue("@ApiLogId", apiLogId);
            cmd.Parameters.AddWithValue("@Response", response);
            cmd.Parameters.AddWithValue("@TimeInMs", timeInMs);

            _sqlDb.ExecuteNonSelect(cmd);
        }
    }
}