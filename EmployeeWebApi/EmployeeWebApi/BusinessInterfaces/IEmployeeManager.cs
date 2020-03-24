using EmployeeWebApi.Models;
using EmployeeWebApi.Models.EmployeeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.BusinessInterfaces
{
    public interface IEmployeeManager
    {
        List<EmployeeViewModel> GetEmployeesByPan(List<string> panList);
        void CreateEmployees(List<EmployeeRequestDetail> detailLst);
        void DeleteEmployees(List<string> panList);
    }
}
