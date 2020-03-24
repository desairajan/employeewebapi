using EmployeeWebApi.Models.EmployeeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.BusinessInterfaces
{
    public interface IEmployeeApiValidationManager
    {
        void AuthorizeEmployeeApiAccess(int sysApiId, string token);

        void ValidateIfTransactionArePresent(EmployeeRequest request);
        List<EmployeeResponseDetail> ValidateEachTransaction(List<EmployeeRequestDetail> reqDetailList);
        List<EmployeeResponseDetail> ValidateEmployeeExistsOrNot(List<EmployeeRequestDetail> reqDetailList);
    }
}
