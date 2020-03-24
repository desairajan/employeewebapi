using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EmployeeWebApi.Models.EmployeeApi
{
    public class EmployeeRequestDetail
    {
        public string TransactionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pan { get; set; }
        [XmlIgnore]
        public DateTime? DateOfBirth { get; set; }
        public string BirthDate { get; set; }
        [XmlIgnore]
        public string Token { get; set; }
    }
}