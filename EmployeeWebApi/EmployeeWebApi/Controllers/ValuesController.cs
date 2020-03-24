using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeWebApi.BusinessInterfaces;

namespace EmployeeWebApi.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ValuesController).Name);
        IValuesManager _valueManager;
        public ValuesController()
        {
            _valueManager = IoC.Resolve<IValuesManager>();
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            _log.Info("Get called");
            return _valueManager.GetValues();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
