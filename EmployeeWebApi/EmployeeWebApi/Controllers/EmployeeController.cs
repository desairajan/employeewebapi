using EmployeeWebApi.BusinessImplementation;
using EmployeeWebApi.BusinessInterfaces;
using EmployeeWebApi.Enums;
using EmployeeWebApi.Models;
using EmployeeWebApi.Models.EmployeeApi;
using EmployeeWebApi.UDFException;
using EmployeeWebApi.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;
using EmployeeWebApi;

namespace EmployeeWebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(EmployeeController).Name);
        private IApiRequestIdCacheManager _apiRequestIdCacheManager;
        private IApiCacheManager _apiCacheManager;
        private IEmployeeManager _employeeManager;
        private IApiLogManager _apiLogManager;
        private IEmployeeApiValidationManager _employeeApiValidationManager;

        private SysApi _api;
        public EmployeeController()
        {
            _apiRequestIdCacheManager = IoC.Resolve<IApiRequestIdCacheManager>();
            _apiCacheManager = IoC.Resolve<IApiCacheManager>();
            _employeeManager = IoC.Resolve<IEmployeeManager>();
            _apiLogManager = IoC.Resolve<IApiLogManager>();
            _employeeApiValidationManager = IoC.Resolve<IEmployeeApiValidationManager>();
        }
        [HttpPost]
        //[Route("GetEmployee")]
        public HttpResponseMessage GetEmployee()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string xmlResponse = string.Empty;
            var xml = Request.Content.ReadAsStringAsync().Result;
            EmployeeResponse response = new EmployeeResponse();
            long? apiLogId = null;
            var request = new EmployeeRequest();

            try
            {
                request = XmlUtils.Deserialize<EmployeeRequest>(xml);
                ModifyRequest(request);
                response = InitializeResponse(request);

                CreateCache();

                //insert into CoreApiLog
                apiLogId = _apiLogManager.InsertIntoApiLog(new ApiLogViewModel() { SysApiId = _api.Id, Request = xml, RequestId = request.RequestId, Token = request.Token });
                

                _apiRequestIdCacheManager.ValidateRequestIdInsertIfNotAndThrowIfExists(_api.Id, request.RequestId);


                _employeeApiValidationManager.AuthorizeEmployeeApiAccess(_api.Id, request.Token);

                _employeeApiValidationManager.ValidateIfTransactionArePresent(request);

                var employeeList = _employeeManager.GetEmployeesByPan(request.EmployeeRequestDetails.Select(i => i.Pan).ToList());

                var empResLst = new List<EmployeeResponseDetail>();
                var empReqDic = request.EmployeeRequestDetails.ToDictionary(i => i.Pan);
                foreach (var employee in employeeList)
                {
                    var empReqObj = empReqDic[employee.Pan];
                    EmployeeResponseDetail empResDetail = new EmployeeResponseDetail();
                    empResDetail.TransactionId = empReqObj.TransactionId;
                    if (!string.IsNullOrEmpty(employee.Description))
                    { 
                        empResDetail.RejectionDetail = new RejectionDetail(RejectionCode.RC6.ToString(), employee.Description);
                        empResDetail.TransactionStatus = ApiStatus.Rejected.ToString();
                    }
                    else
                    {
                        empResDetail.FirstName = employee.FirstName;
                        empResDetail.LastName = employee.LastName;
                        empResDetail.Pan = employee.Pan;
                        empResDetail.BirthDate = employee.BirthDate.HasValue ? employee.BirthDate.Value.ToString("dd-MM-yyyy") : null;
                        empResDetail.TransactionStatus = ApiStatus.Success.ToString();
                    }

                    empResLst.Add(empResDetail);
                }

                response.EmployeeResponseDetails = empResLst;
                response.RequestStatus = ApiStatus.Success.ToString();

            }
            catch (ApiRejectionException apiEx)
            {
                _log.Error("Error in GetEmployee ", apiEx);
                PopulateRejectedResponse(response, apiEx.Code,apiEx.Message);
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetEmployee ", ex);

                PopulateRejectedResponse(response, RejectionCode.RC5.ToString(), "Unexpected exception has occured with detail - " + ex.Message);
            }
            finally
            {
                xmlResponse = XmlUtils.Serialize<EmployeeResponse>(response);
                sw.Stop();
                var totalTimeTakenInMs = (long)sw.Elapsed.TotalMilliseconds;
                //update CoreApiLog async
                Task.Run(() => UpdateApiLog(apiLogId.Value, xmlResponse, totalTimeTakenInMs));
            }

            return new HttpResponseMessage() { Content = new StringContent(xmlResponse, System.Text.Encoding.UTF8, "application/xml") };

        }

        [HttpPost]
        //[Route("CreateEmployee")]
        public HttpResponseMessage CreateEmployee()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string xmlResponse = string.Empty;
            var xml = Request.Content.ReadAsStringAsync().Result;
            EmployeeResponse response = new EmployeeResponse();
            long? apiLogId = null;
            var request = new EmployeeRequest();
            try
            {
                request = XmlUtils.Deserialize<EmployeeRequest>(xml);
                ModifyRequest(request);
                response = InitializeResponse(request);



                CreateCache();

                //insert into CoreApiLog
                apiLogId = _apiLogManager.InsertIntoApiLog(new ApiLogViewModel() { SysApiId = _api.Id, Request = xml, RequestId = request.RequestId, Token = request.Token });



                _apiRequestIdCacheManager.ValidateRequestIdInsertIfNotAndThrowIfExists(_api.Id, request.RequestId);


                _employeeApiValidationManager.AuthorizeEmployeeApiAccess(_api.Id, request.Token);

                _employeeApiValidationManager.ValidateIfTransactionArePresent(request);

                //validate if FirstName and Pan are provided and BirthDate is as per format
                var empResDetailList = _employeeApiValidationManager.ValidateEachTransaction(request.EmployeeRequestDetails);

                var validationSuccessList = empResDetailList.Where(i => i.TransactionStatus == ApiStatus.Success.ToString()).Select(i => i.TransactionId).ToList();

                var createEmpList = request.EmployeeRequestDetails.FindAll(i => validationSuccessList.Contains(i.TransactionId));

                _employeeManager.CreateEmployees(createEmpList);

                response.EmployeeResponseDetails = empResDetailList;
                response.RequestStatus = ApiStatus.Success.ToString();

            }
            catch (ApiRejectionException apiEx)
            {
                _log.Error("Error in CreateEmployee ", apiEx);
                PopulateRejectedResponse(response, apiEx.Code, apiEx.Message);
            }
            catch (Exception ex)
            {
                _log.Error("Error in CreateEmployee ", ex);

                PopulateRejectedResponse(response, RejectionCode.RC5.ToString(), "Unexpected exception has occured with detail - " + ex.Message);
            }
            finally
            {
                xmlResponse = XmlUtils.Serialize<EmployeeResponse>(response);
                sw.Stop();
                var totalTimeTakenInMs = (long)sw.Elapsed.TotalMilliseconds;

                //update CoreApiLog async
                Task.Run(()=>UpdateApiLog(apiLogId.Value, xmlResponse, totalTimeTakenInMs));
                
            }

            return new HttpResponseMessage() { Content = new StringContent(xmlResponse, System.Text.Encoding.UTF8, "application/xml") };
        }

        private static void ModifyRequest(EmployeeRequest request)
        {
            if (string.IsNullOrEmpty(request.RequestId))
                request.RequestId = Guid.NewGuid().ToString();
            request.EmployeeRequestDetails.ForEach(i => i.Token = request.Token);
        }

        [HttpPost]
        //[Route("DeleteEmployee")]
        public HttpResponseMessage DeleteEmployee()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string xmlResponse = string.Empty;
            var xml = Request.Content.ReadAsStringAsync().Result;
            EmployeeResponse response = new EmployeeResponse();
            long? apiLogId = null;
            var request = new EmployeeRequest();
            try
            {
                request = XmlUtils.Deserialize<EmployeeRequest>(xml);
                ModifyRequest(request);
                response = InitializeResponse(request);

                CreateCache();

                //insert into CoreApiLog
                apiLogId = _apiLogManager.InsertIntoApiLog(new ApiLogViewModel() { SysApiId = _api.Id, Request = xml, RequestId = request.RequestId, Token = request.Token });
                

                _apiRequestIdCacheManager.ValidateRequestIdInsertIfNotAndThrowIfExists(_api.Id, request.RequestId);


                _employeeApiValidationManager.AuthorizeEmployeeApiAccess(_api.Id, request.Token);

                _employeeApiValidationManager.ValidateIfTransactionArePresent(request);

                var resDetailList = _employeeApiValidationManager.ValidateEmployeeExistsOrNot(request.EmployeeRequestDetails);


                var validationSuccessList = resDetailList.Where(i => i.TransactionStatus == ApiStatus.Success.ToString()).Select(i => i.TransactionId).ToList();

                var deleteEmpPanList = request.EmployeeRequestDetails.FindAll(i => validationSuccessList.Contains(i.TransactionId)).Select(i=>i.Pan).ToList();

                //for all valid pan delete
                _employeeManager.DeleteEmployees(deleteEmpPanList);

                response.EmployeeResponseDetails = resDetailList;
                response.RequestStatus = ApiStatus.Success.ToString();
                

            }
            catch (ApiRejectionException apiEx)
            {
                _log.Error("Error in DeleteEmployee ", apiEx);
                PopulateRejectedResponse(response, apiEx.Code, apiEx.Message);
            }
            catch (Exception ex)
            {
                _log.Error("Error in DeleteEmployee ", ex);

                PopulateRejectedResponse(response, RejectionCode.RC5.ToString(), "Unexpected exception has occured with detail - " + ex.Message);
            }
            finally
            {
                xmlResponse = XmlUtils.Serialize<EmployeeResponse>(response);
                sw.Stop();
                var totalTimeTakenInMs = (long)sw.Elapsed.TotalMilliseconds;

                //update CoreApiLog async
                Task.Run(() => UpdateApiLog(apiLogId.Value, xmlResponse, totalTimeTakenInMs));

            }

            return new HttpResponseMessage() { Content = new StringContent(xmlResponse, System.Text.Encoding.UTF8, "application/xml") };
        }

        private void CreateCache()
        {
            try
            {
                _apiCacheManager.CacheApi();
                _apiCacheManager.CacheApiAccessToken();
                _api = ApiCacheManager.apiDic.FirstOrDefault(i => i.Key.ToLower().Equals("a1")).Value;
            }
            catch (Exception ex)
            {
                _log.Error("Error in CreateCache - ", ex);
                throw;
            }
        }

        private void UpdateApiLog(long apiLogId, string response, long timeInMs)
        {
            try
            {
                _apiLogManager.UpdateApiLogWithResponseAndTime(apiLogId, response, timeInMs);
            }
            catch (Exception ex)
            {
                _log.Error("Error in UpdateApiLog ", ex);
            }

        }
        
        private void PopulateRejectedResponse(EmployeeResponse response, string rejCode, string rejDescription)
        {
            response.RejectionDetail = new RejectionDetail(rejCode, rejDescription);
            response.RequestStatus = ApiStatus.Rejected.ToString();
        }
        
        private EmployeeResponse InitializeResponse(EmployeeRequest req)
        {
            EmployeeResponse res = new EmployeeResponse();
            res.RequestId = req.RequestId;

            return res;
        }
    }
}
