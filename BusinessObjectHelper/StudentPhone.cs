using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using EmailHelper;

namespace BusinessObjectHelper
{
    public class StudentPhone : HeaderData
    {
        private Guid _studentId;
        private string _phone;
        private Guid _phoneTypeId;

        public Guid StudentId
        {
            get { return _studentId; }
            set { if (_studentId != value) { _studentId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public string Phone
        {
            get { return _phone; }
            set { { _phone = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public Guid PhoneTypeId
        {
            get { return _phoneTypeId; }
            set { { _phoneTypeId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        private Boolean Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentPhoneInsert";
                db.Command.Parameters.Clear();
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@phone", SqlDbType.VarChar).Value = _phone;
                db.Command.Parameters.Add("@phonetypeid", SqlDbType.UniqueIdentifier).Value = _phoneTypeId;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private Boolean Update(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentPhoneUpdate";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@phone", SqlDbType.VarChar).Value = _phone;
                db.Command.Parameters.Add("@phonetypeid", SqlDbType.UniqueIdentifier).Value = _phoneTypeId;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private Boolean Delete(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentPhoneDelete";
                base.Initialize(db.Command, base.Id);
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private Boolean IsValid()
        {
            Boolean Result = true;
            try
            {
                if (_phone.Trim() == string.Empty)
                    Result = false;
                if (_phone.Length > 50)
                    Result = false;
            }
            catch (Exception ex) { throw; }
            return Result;
        }

        public Boolean IsSavable()
        {
            Boolean Result = true;
            if (base.IsDirty == true && IsValid() == true)
                Result = true;
            else
                Result = false;
            return Result;
        }

        public StudentPhone Save(Database db, Guid parentid)
        {
            _studentId = parentid;
            Boolean result = false;
            if (base.IsNew == true && IsSavable() == true)
                result = Insert(db);
            else if (base.Deleted == true)
                result = Delete(db);
            else if (base.IsNew == false && IsSavable() == true)
                result = Update(db);
            base.IsDirty = false;
            base.IsNew = false;
            return this;
        }

        public void InitializeBusiniessData(DataRow dr)
        {
            _studentId = (Guid)(dr["studentid"]);
            _phone = dr["phone"].ToString();
            _phoneTypeId = (Guid)(dr["phonetypeid"]);
        }
    }

}
