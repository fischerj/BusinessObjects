using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using EmailHelper;

namespace BusinessObjectHelper
{
    public class StudentClass : HeaderData
    {
        private Guid _studentId;
        private Guid _classId;
        private Class _class;

        public Class Class
        {
            get
            {
                _class = new Class();
                _class = _class.GetById(_classId);
                return _class;
            }
        }
        public Guid StudentId
        {
            get { return _studentId; }
            set { if (_studentId != value) { _studentId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public Guid ClassId
        {
            get { return _classId; }
            set { { _classId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        private Boolean Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentClassInsert";
                db.Command.Parameters.Clear();
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@classid", SqlDbType.UniqueIdentifier).Value = _classId;
                db.ExecuteNonQueryWithTransaction();
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
                db.Command.CommandText = "tblStudentClassUpdate";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@classid", SqlDbType.UniqueIdentifier).Value = _classId;
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
                db.Command.CommandText = "tblStudentClassDelete";
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
                if (_classId == Guid.Empty)
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

        public StudentClass Save(Database db, Guid parentid)
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
            _classId = (Guid)(dr["classId"]);
        }
    }

}
