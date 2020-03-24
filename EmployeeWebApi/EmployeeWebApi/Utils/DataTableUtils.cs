using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Utils
{
    public class DataTableUtils
    {
        public static T GetValue<T>(DataRow row, string columnName)
        {
            object val = row[columnName];
            Type t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (val == null || val == DBNull.Value || (string.IsNullOrEmpty(val.ToString()) && t!=typeof(string)))
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);

            }
            return (T)Convert.ChangeType(val, t);
        }
    }
}