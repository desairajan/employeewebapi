using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models
{
    public class RejectionDetail
    {
        public RejectionDetail()
        { }
        public RejectionDetail(string rejectionCode,string rejDescription)
        {
            RejectionCode = rejectionCode;
            RejectionDescription = rejDescription;
        }
        public string RejectionCode { get; set; }
        public string RejectionDescription { get; set; }
    }
}