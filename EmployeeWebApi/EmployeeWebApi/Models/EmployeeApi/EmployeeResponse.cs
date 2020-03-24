using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models.EmployeeApi
{
    public class EmployeeResponse
    {
        public EmployeeResponse()
        {
            EmployeeResponseDetails = new List<EmployeeResponseDetail>();
        }
        public string RequestId { get; set; }
        public string RequestStatus { get; set; }
        public RejectionDetail RejectionDetail { get; set; }
        public List<EmployeeResponseDetail> EmployeeResponseDetails { get; set; }
    }
}