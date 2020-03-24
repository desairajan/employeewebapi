using EmployeeWebApi.BusinessInterfaces;
using EmployeeWebApi.UDFException;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.BusinessImplementation
{
    public class ApiRequestIdCacheManager: IApiRequestIdCacheManager
    {
        private static ConcurrentDictionary<Tuple<int, string>, string> _requestIdCacheDic;
        public ApiRequestIdCacheManager()
        {
            if (_requestIdCacheDic == null)
                _requestIdCacheDic = new ConcurrentDictionary<Tuple<int, string>, string>();
        }

        public bool ValidateRequestIdInsertIfNotAndThrowIfExists(int apiId,string requestId)
        {
            var key = new Tuple<int, string>(apiId, requestId);

            if (_requestIdCacheDic.ContainsKey(key))
                throw new ApiRejectionException(Enums.RejectionCode.RC1,"RequestId should be unique");

            return _requestIdCacheDic.TryAdd(key, requestId);
        }
    }
}