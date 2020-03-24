﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.BusinessInterfaces
{
    public interface IApiRequestIdCacheManager
    {
        bool ValidateRequestIdInsertIfNotAndThrowIfExists(int apiId, string requestId);
    }
}
