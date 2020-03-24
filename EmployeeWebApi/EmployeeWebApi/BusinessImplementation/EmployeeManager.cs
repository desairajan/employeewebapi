using EmployeeWebApi.BusinessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeWebApi.Models;
using EmployeeWebApi.DAL.Interface;
using EmployeeWebApi.Models.EmployeeApi;
using System.Data;

namespace EmployeeWebApi.BusinessImplementation
{
    public class EmployeeManager : IEmployeeManager
    {
        private IEmployeeDao _employeeDao;
        public EmployeeManager(IEmployeeDao employeeDao)
        {
            _employeeDao = employeeDao;
        }
        public List<EmployeeViewModel> GetEmployeesByPan(List<string> panList)
        {
            return _employeeDao.GetEmployeeByPan(panList);
        }

        public void CreateEmployees(List<EmployeeRequestDetail> detailLst)
        {
            if (detailLst.Any())
            {
                DataTable dt = new DataTable();
                dt.TableName = "CoreEmployee";

                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("LastName", typeof(string));
                dt.Columns.Add("Pan", typeof(string));
                dt.Columns.Add("BirthDate", typeof(DateTime)).AllowDBNull = true;
                dt.Columns.Add("AddedBy", typeof(string));
                dt.Columns.Add("AddedOn", typeof(DateTime));

                DateTime now = DateTime.Now;
                foreach (var detail in detailLst)
                {
                    DataRow row = dt.NewRow();

                    row["FirstName"] = detail.FirstName;
                    row["LastName"] = detail.LastName;
                    row["Pan"] = detail.Pan;
                    row["BirthDate"] = detail.DateOfBirth.HasValue ? (object)detail.DateOfBirth.Value : DBNull.Value;
                    row["AddedBy"] = detail.Token;
                    row["AddedOn"] = now;

                    dt.Rows.Add(row);
                }

                _employeeDao.CreateEmployees(dt);
            }
        }

        public void DeleteEmployees(List<string> panList)
        {
            _employeeDao.DeleteEmployees(panList);
        }
    }
}