using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
namespace BusinessObjectHelper
{
    public class AddressType : HeaderData
    {
        #region Private Members
        private string _Type = string.Empty;
        #endregion

        #region Public Properties
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    base.IsDirty = true;
                    Boolean savable = IsSavable();
                    RaiseEvent(savable);

                }
            }
        }

        #endregion

        #region Private Methods
        private Boolean Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblAddressTypeInsert";
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@Type", SqlDbType.VarChar).Value = _Type;
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private Boolean Update(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblAddressTypeUpdate";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@Type", SqlDbType.VarChar).Value = _Type;
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Boolean Delete(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblAddressTypeDelete";
                base.Initialize(db.Command, base.Id);
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Boolean IsValid()
        {
            Boolean Result = true;
            if (_Type.Trim() == string.Empty)
            {
                Result = false;
            }

            if (_Type.Trim().Length > 50)
            {
                Result = false;
            }

            return Result;
        }

        #endregion

        #region Public Methods
        public Boolean IsSavable()
        {
            Boolean Result = true;
            if (base.IsDirty == true && IsValid() == true)
                Result = true;
            else
                Result = false;

            return Result;
        }

        public AddressType Save()
        {
            Database db = new Database("student");
            Boolean result = false;
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
            base.IsDirty = false;
            base.IsNew = false;

            return this;
        }

        public AddressType GetById(Guid Id)
        {
            Database db = new Database("Student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblAddressTypeGetById";
                db.Command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    base.Initialize(dr);
                    InitializeBusinessData(dr);
                    return this;
                }
                else
                    throw new Exception("AddressType not found");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void InitializeBusinessData(DataRow dr)
        {
            _Type = dr["Type"].ToString();
        }

        #endregion


    }
}
