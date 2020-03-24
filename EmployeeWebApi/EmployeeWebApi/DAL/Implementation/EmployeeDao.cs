using EmployeeWebApi.DAL.Interface;
using EmployeeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EmployeeWebApi;
using EmployeeWebApi.Utils;

namespace EmployeeWebApi.DAL.Implementation
{
    public class EmployeeDao : IEmployeeDao
    {
        private ISqlDatabase _sqlDatabase;
        public EmployeeDao()
        {
            _sqlDatabase = IoC.Resolve<ISqlDatabase>();
        }

        public List<EmployeeViewModel> GetEmployeeByPan(List<string> panList)
        {
            SqlCommand cmd = new SqlCommand("dbo.CoreEmployee_GetByPan");
            cmd.Parameters.AddWithValue("@Pan", string.Join(",", panList));
            var dt = _sqlDatabase.ExecuteSelect(cmd);

            List<EmployeeViewModel> empList = new List<EmployeeViewModel>();

            foreach (DataRow row in dt.Rows)
            {
                EmployeeViewModel empModel = new EmployeeViewModel();
                empModel.Id = DataTableUtils.GetValue<long?>(row, "CoreEmployeeId");
                empModel.Pan = DataTableUtils.GetValue<string>(row,"Pan");
                empModel.FirstName = DataTableUtils.GetValue<string>(row,"FirstName");
                empModel.LastName = DataTableUtils.GetValue<string>(row,"LastName");
                empModel.BirthDate = DataTableUtils.GetValue<DateTime?>(row,"BirthDate");
                empModel.Description = DataTableUtils.GetValue<string>(row, "Description");

                empList.Add(empModel);
            }

            return empList;
        }
        
        public void CreateEmployees(DataTable dt)
        {
            _sqlDatabase.BulkUpload(dt);
        }
        public void DeleteEmployees(List<string> panList)
        {
            SqlCommand cmd = new SqlCommand("dbo.CoreEmployee_DeleteByPan");
            cmd.Parameters.AddWithValue("@Pan", string.Join(",", panList));
            _sqlDatabase.ExecuteNonSelect(cmd);
        }
    }
}