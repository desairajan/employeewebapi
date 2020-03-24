using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeWebApi.BusinessInterfaces;

namespace EmployeeWebApi.BusinessImplementation
{
    public class ValuesManager: IValuesManager
    {
        public IEnumerable<string> GetValues()
        {
            return new string[] { "value1", "value2" };
        }
    }
}