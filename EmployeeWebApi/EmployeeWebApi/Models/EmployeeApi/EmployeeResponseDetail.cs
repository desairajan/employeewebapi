using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models.EmployeeApi
{
    public class EmployeeResponseDetail
    {
        public string TransactionId { get; set; }
        public string TransactionStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Pan { get; set; }
        public RejectionDetail RejectionDetail { get; set; }
    }
}