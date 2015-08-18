using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using EmailHelper;

namespace BusinessObjectHelper
{
   public class StudentComment : HeaderData
    {
        private Guid _studentId;
        private string _subject;
        private string _comment;
        private Guid _categoryId;
        private Guid _parentId;
        private DateTime _dateCreated = DateTime.MaxValue;

        public Guid CategoryId
        {
            get { return _categoryId; }
            set { if (_categoryId != value) { _categoryId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public Guid StudentId
        {
            get { return _studentId; }
            set { if (_studentId != value) { _studentId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { if (_dateCreated != value) { _dateCreated = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        public Guid ParentId
        {
            get { return _parentId; }
            set { if (_parentId != value) { _parentId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public string Subject
        {
            get { return _subject; }
            set {  { _subject = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public string Comment
        {
            get { return _comment; }
            set {  { _comment = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        private Boolean Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentCommentInsert";
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value=_studentId;
                db.Command.Parameters.Add("@parentid", SqlDbType.UniqueIdentifier).Value = _parentId;
                db.Command.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier).Value = _categoryId;
                db.Command.Parameters.Add("@subject", SqlDbType.VarChar).Value = _subject;
                db.Command.Parameters.Add("@commentData", SqlDbType.VarChar).Value = _comment;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch { throw; }
        }

        private Boolean Update(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentCommentUpdate";
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@parentid", SqlDbType.UniqueIdentifier).Value = _parentId;
                db.Command.Parameters.Add("@parentid", SqlDbType.UniqueIdentifier).Value = _parentId;
                db.Command.Parameters.Add("@subject", SqlDbType.VarChar).Value = _subject;
                db.Command.Parameters.Add("@commentData", SqlDbType.VarChar).Value = _comment;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch { throw; }
        }

        private Boolean Delete(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentCommentDelete";
                base.Initialize(db.Command, base.Id);
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch { throw; }
        }

        private Boolean IsValid()
        {
            Boolean Result = true;
            try
            {
                if (_subject.Trim() == string.Empty)
                    Result = false;
                if (_comment.Trim() == string.Empty)
                    Result = false;
                if (_subject.Length > 50)
                    Result = false;
                if (_comment.Length > 8000)
                    Result = false;
            }
            catch { }
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

        public StudentComment Save(Database db, Guid parentId)
        {
            _studentId = parentId;
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
            _subject = dr["subject"].ToString();
            _comment = dr["commentData"].ToString();
            _parentId = (Guid)dr["parentId"];
            _dateCreated = (DateTime) dr["DateCreated"];
        } 
    }
}
