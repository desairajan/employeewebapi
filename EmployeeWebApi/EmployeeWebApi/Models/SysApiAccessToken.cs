using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models
{
    public class SysApiAccessToken
    {
        public int SysApiId { get; set; }
        public string ApiCode { get; set; }
        public string Token { get; set; }
    }
}