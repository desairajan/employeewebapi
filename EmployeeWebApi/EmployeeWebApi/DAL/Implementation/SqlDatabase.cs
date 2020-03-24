using EmployeeWebApi.DAL.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.DAL.Implementation
{
    public class SqlDatabase: ISqlDatabase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SqlDatabase).Name);
        private static string _connectionString;
        //to be called on application start
        public void InitialiseConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataTable ExecuteSelect(SqlCommand cmd)
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = int.MaxValue;
            DataTable dt = new DataTable();
            try
            {
                _log.Info("Executing command " + cmd.CommandText);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    var transaction = conn.BeginTransaction();
                    adapter.SelectCommand.Transaction = transaction;
                    adapter.Fill(dt);

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error in ExecuteSelect - " + ex.Message, ex);
                throw;
            }
            return dt;
        }

        public Object ExecuteScalar(SqlCommand cmd)
        {
            Object result = null;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = int.MaxValue;
            try
            {
                _log.Info("Executing command " + cmd.CommandText);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;
                    result = cmd.ExecuteScalar();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error in ExecuteScalar - " + ex.Message, ex);
                throw;
            }

            return result;
        }

        public void ExecuteNonSelect(SqlCommand cmd)
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = int.MaxValue;

            try
            {
                _log.Info("Executing command " + cmd.CommandText);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error in ExecuteNonSelect - " + ex.Message, ex);
                throw;
            }
        }

        public void BulkUpload(DataTable dt)
        {
            _log.Info("Uploading to table " + dt.TableName + " with rows " + dt.Rows.Count);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(conn))
                {
                    foreach (DataColumn item in dt.Columns)
                    {
                        copy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                    }

                    copy.DestinationTableName = dt.TableName;
                    copy.BulkCopyTimeout = int.MaxValue;
                    try
                    {
                        copy.WriteToServer(dt);
                    }
                    catch(Exception ex)
                    {
                        _log.Error("Error while uploading to " + dt.TableName);
                        throw;
                    }
                }
            }
        }
    }
}