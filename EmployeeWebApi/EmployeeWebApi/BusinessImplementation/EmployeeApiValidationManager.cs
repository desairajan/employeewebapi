using EmployeeWebApi.BusinessInterfaces;
using EmployeeWebApi.Models.EmployeeApi;
using EmployeeWebApi.UDFException;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.BusinessImplementation
{
    public class EmployeeApiValidationManager: IEmployeeApiValidationManager
    {
        private IEmployeeManager _employeeManager;
        public EmployeeApiValidationManager(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        public void AuthorizeEmployeeApiAccess(int sysApiId, string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ApiRejectionException(Enums.RejectionCode.RC4, "Api access token should be provided");

            if (!ApiCacheManager.apiAccessDic.ContainsKey(new Tuple<int, string>(sysApiId, token)))
                throw new ApiRejectionException(Enums.RejectionCode.RC3, "Unauthorized Api access");
        }

        public void ValidateIfTransactionArePresent(EmployeeRequest request)
        {
            if (request.EmployeeRequestDetails == null || !request.EmployeeRequestDetails.Any())
            {
                throw new ApiRejectionException(Enums.RejectionCode.RC2, "Empoyee Transactions not found in Request");
            }
        }

        public List<EmployeeResponseDetail> ValidateEachTransaction(List<EmployeeRequestDetail> reqDetailList)
        {
            List<EmployeeResponseDetail> responseDetailList = new List<EmployeeResponseDetail>();

            var existingEmployeeDic = _employeeManager.GetEmployeesByPan(reqDetailList.Select(i => i.Pan).ToList()).ToDictionary(i=>i.Pan);


            foreach (var reqDetail in reqDetailList)
            {
                EmployeeResponseDetail res = new EmployeeResponseDetail();
                res.TransactionId = reqDetail.TransactionId;

                try
                {
                    if (string.IsNullOrEmpty(reqDetail.FirstName))
                        throw new ApiRejectionException(Enums.RejectionCode.RC7, "FirstName is mandatory");

                    if (string.IsNullOrEmpty(reqDetail.Pan))
                        throw new ApiRejectionException(Enums.RejectionCode.RC8, "Pan is mandatory");

                    DateTime? dateOfBirth = GetValidDateTime(reqDetail.BirthDate);

                    if (dateOfBirth == null && !string.IsNullOrEmpty(reqDetail.BirthDate))
                        throw new ApiRejectionException(Enums.RejectionCode.RC9, "Birth Date should be in format dd-MM-yyyy");

                    if (existingEmployeeDic.ContainsKey(reqDetail.Pan) && string.IsNullOrEmpty(existingEmployeeDic[reqDetail.Pan].Description))
                        throw new ApiRejectionException(Enums.RejectionCode.RC10, "Employee already exists");


                    reqDetail.DateOfBirth = dateOfBirth;
                    res.TransactionStatus = Enums.ApiStatus.Success.ToString();
                }
                catch (ApiRejectionException ex)
                {
                    res.TransactionStatus = Enums.ApiStatus.Rejected.ToString();
                    res.RejectionDetail = new Models.RejectionDetail(ex.Code, ex.Message);
                }
                responseDetailList.Add(res);
            }
            return responseDetailList;
        }

        private DateTime? GetValidDateTime(string date)
        {
           return string.IsNullOrEmpty(date) ? (DateTime?)null : DateTime.ParseExact(date, new[] { "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
        }

        public List<EmployeeResponseDetail> ValidateEmployeeExistsOrNot(List<EmployeeRequestDetail> reqDetailList)
        {
            List<EmployeeResponseDetail> resDetailList = new List<EmployeeResponseDetail>();

            var existingEmployeeDic = _employeeManager.GetEmployeesByPan(reqDetailList.Select(i => i.Pan).ToList()).ToDictionary(i => i.Pan);

            foreach (var reqDetail in reqDetailList)
            {
                EmployeeResponseDetail resDetail = new EmployeeResponseDetail();
                resDetail.TransactionId = reqDetail.TransactionId;
                try
                {
                    if (existingEmployeeDic.ContainsKey(reqDetail.Pan) && !string.IsNullOrEmpty(existingEmployeeDic[reqDetail.Pan].Description))
                        throw new ApiRejectionException(Enums.RejectionCode.RC11, "Employee doesn't exist");
                    resDetail.TransactionStatus = Enums.ApiStatus.Success.ToString();

                }
                catch (ApiRejectionException ex)
                {
                    resDetail.RejectionDetail = new Models.RejectionDetail(ex.Code, ex.Message);
                    resDetail.TransactionStatus = Enums.ApiStatus.Rejected.ToString();
                }
                resDetailList.Add(resDetail);
            }
            return resDetailList;
        }
    }
}