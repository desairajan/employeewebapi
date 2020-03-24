using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models
{
    public class ApiLogViewModel
    {
        public int SysApiId { get; set; }
        public string RequestId { get; set; }
        public string Token { get; set; }
        public string Request { get; set; }
    }
}