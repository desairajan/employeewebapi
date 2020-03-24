using EmployeeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.DAL.Interface
{
    public interface IEmployeeDao
    {
        List<EmployeeViewModel> GetEmployeeByPan(List<string> panList);
        void CreateEmployees(DataTable dt);
        void DeleteEmployees(List<string> panList);
    }
}
