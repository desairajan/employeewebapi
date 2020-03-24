using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models
{
    public class EmployeeViewModel
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pan { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Description { get; set; }
    }
}