using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ConfigurationHelper;

namespace DatabaseHelper
{
    public class Database
    {
        #region Private Members
        SqlConnection _cn;
        SqlCommand _cmd;
        SqlDataAdapter _da;
        DataTable _dt;
        string _connectionName = string.Empty;
        #endregion

        #region Public Properties
        public SqlCommand Command
        {
            get
            {
                return _cmd;
            }
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public void BeginTransaction()
        {
            _cn.ConnectionString = Configuration.GetConnectionString(_connectionName);
            _cn.Open();
            _cmd.Transaction = _cn.BeginTransaction();
        }
        public void EndTransaction()
        {
            _cmd.Transaction.Commit();
            _cn.Close();
        }
        public void RollBackTransaction()
        {
            _cmd.Transaction.Rollback();
            _cn.Close();
        }
        public SqlCommand ExecuteNonQueryWithTransaction()
        {
            _cmd.ExecuteNonQuery();
            return _cmd;
        }
        public SqlCommand ExecuteNonQuery()
        {
            try
            {
                _cn.ConnectionString = Configuration.GetConnectionString(_connectionName);
                _cn.Open();
                _cmd.ExecuteNonQuery();
                _cn.Close();
            }
            catch (Exception e)
            {
                _cn.Close();
                throw;
            }
            return _cmd;
        }

        public DataTable ExecuteQuery()
        {
            try
            {
                _cn.ConnectionString =
                    Configuration.GetConnectionString(_connectionName);
                _cn.Open();
                _da.Fill(_dt);
                _cn.Close();
            }
            catch (Exception e)
            {
                _cn.Close();
                throw;
            }
            return _dt;
        }
        #endregion

        #region Construction
        public Database(string connectionName)
        {
            _connectionName = connectionName;
            _cn = new SqlConnection();
            _cmd = new SqlCommand();
            _cmd.Connection = _cn;
            _da = new SqlDataAdapter();
            _da.SelectCommand = _cmd;
            _dt = new DataTable();
        }
        #endregion
    }
}
