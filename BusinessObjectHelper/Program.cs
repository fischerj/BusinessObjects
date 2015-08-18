using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
namespace BusinessObjectHelper
{
    public class Program : HeaderData
    {
        #region Private Members
        private string _Name = string.Empty;
        #endregion

        #region Public Properties
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
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
                db.Command.CommandText = "tblProgramInsert";
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@Name", SqlDbType.VarChar).Value = _Name;
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
                db.Command.CommandText = "tblProgramUpdate";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@Name", SqlDbType.VarChar).Value = _Name;
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
                db.Command.CommandText = "tblProgramDelete";
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
            if (_Name.Trim() == string.Empty)
            {
                Result = false;
            }

            if (_Name.Trim().Length > 50)
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

        public Program Save()
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

        public Program GetById(Guid Id)
        {
            Database db = new Database("Student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblProgramGetById";
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
                    throw new Exception("Program not found");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void InitializeBusinessData(DataRow dr)
        {
            _Name = dr["Name"].ToString();
        }

        #endregion


    }
}
