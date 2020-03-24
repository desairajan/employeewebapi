using EmployeeWebApi.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeWebApi.Models;
using EmployeeWebApi;
using System.Data.SqlClient;
using System.Data;
using EmployeeWebApi.Utils;

namespace EmployeeWebApi.DAL.Implementation
{
    public class SysApiDao : ISysApiDao
    {
        private ISqlDatabase _sqlDatabase;
        public SysApiDao()
        {
            _sqlDatabase = IoC.Resolve<ISqlDatabase>();
        }
        public List<SysApi> GetAllSysApi()
        {
            SqlCommand cmd = new SqlCommand("dbo.SysApi_GetAllApi");
            var dt = _sqlDatabase.ExecuteSelect(cmd);

            List<SysApi> apiList = new List<SysApi>();

            foreach (DataRow row in dt.Rows)
            {
                SysApi api = new SysApi();
                api.Id = (int)row["SysApiId"];
                api.Name = row["Name"].ToString();
                api.Code = row["Code"].ToString();
                apiList.Add(api);
            }

            return apiList;
        }

        public List<SysApiAccessToken> GetApiAccessToken()
        {
            SqlCommand cmd = new SqlCommand("dbo.SysApi_GetAccessTokens");
            var dt = _sqlDatabase.ExecuteSelect(cmd);

            List<SysApiAccessToken> accessList = new List<SysApiAccessToken>();

            foreach (DataRow row in dt.Rows)
            {
                var accessToken = new SysApiAccessToken();
                accessToken.SysApiId = DataTableUtils.GetValue<int>(row,"SysApiId");
                accessToken.ApiCode = DataTableUtils.GetValue<string>(row, "Code");
                accessToken.Token = DataTableUtils.GetValue<string>(row, "Token");
                accessList.Add(accessToken);
            }

            return accessList;
        }
    }
}