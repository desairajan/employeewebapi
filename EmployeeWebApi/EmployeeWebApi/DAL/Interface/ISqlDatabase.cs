using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.DAL.Interface
{
    public interface ISqlDatabase
    {
        void InitialiseConnectionString(string connectionString);
        DataTable ExecuteSelect(SqlCommand cmd);
        Object ExecuteScalar(SqlCommand cmd);
        void ExecuteNonSelect(SqlCommand cmd);
        void BulkUpload(DataTable dt);
    }
}
