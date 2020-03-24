using EmployeeWebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.UDFException
{
    public class ApiRejectionException:Exception
    {
        public ApiRejectionException(RejectionCode rejectionCode,string rejectionDescription):base(rejectionDescription)
        {
            RejectionCode = rejectionCode;
        }
        public string Code { get { return RejectionCode.ToString(); } }
        public RejectionCode RejectionCode { get; set; }
    }
}