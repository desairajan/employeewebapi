using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models.EmployeeApi
{
    public class EmployeeRequest
    {
        public string Token { get; set; }
        public string RequestId { get; set; }
        public List<EmployeeRequestDetail> EmployeeRequestDetails { get; set; }
    }
}