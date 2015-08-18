using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;

namespace BusinessObjectHelper
{
    class Client : HeaderData
    {
        #region Private Members

        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;

        #endregion

        #region Public Properties
        public string FirstName
        {
            get 
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }

        #endregion

        #region Private Methods

        private bool Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblClient_INSERT";
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = _FirstName;
                db.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = _LastName;
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool Update(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblClient_UPDATE";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = _FirstName;
                db.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = _LastName;
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool Delete(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblClient_DELETE";
                base.Initialize(db.Command, base.Id);
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool IsValid()
        {
            bool result = true;
            if (_FirstName == string.Empty)
            {
                result = false;
            }

            if (_LastName == string.Empty)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Public Methods
        public Client GetById(Guid id)
        {
            Database db = new Database("Customer");
            db.Command.CommandType = CommandType.StoredProcedure;
            db.Command.CommandText = "tblClient_GetById";
            base.Initialize(db.Command, id);
            DataTable dt=db.ExecuteQuery();
            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                InitializeBusinessData(dr);
                base.Initialize(dr);
            }
            return this;
        }

        public Client Save()
        {
            bool result = true;
            Database db = new Database("Customer");
            if (base.IsNew == true && IsSavable() == true)
            {
                result = Insert(db);
            }
            else if (base.Deleted == true)
            {
                result = Delete(db);
            }
            else if (base.IsNew == false && IsSavable() == true)
            {
                result = Update(db);
            }
            base.IsNew = false;
            base.IsDirty = false;

            return this;

        }

        public bool IsSavable()
        {
            bool result = false;
            if (base.IsDirty == true && IsValid() == true)
                result = true;
            return result;
        }

        public void InitializeBusinessData(DataRow dr)
        {
            _FirstName = dr["FirstName"].ToString();
            _LastName = dr["LastName"].ToString();
        }
        #endregion

        #region Events
        #endregion

        #region Construction
        public Client()
        {

        }
        #endregion
    }
}
