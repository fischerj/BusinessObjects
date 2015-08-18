using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
   public class StudentCommentList : Event
    {
        private BindingList<StudentComment> _list;

        public BindingList<StudentComment> List
        { get { return _list; } }

        public Boolean Save(Database db, Guid parentId)
        {
            Boolean result = true;
            foreach (StudentComment s in _list)
                if (s.IsSavable() == true)
                {
                    s.Save(db, parentId);
                    if (s.IsNew == true)
                    {
                        result = false;
                        break;
                    }
                }
            return result;

        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (StudentComment c in _list)
            {
                if (c.IsSavable() == true)
                {
                    result = true;
                    break;
                }
            }
            return result;

        }
        public StudentCommentList GetByStudentId(Guid studentId)
        {
            _list.Clear();
            Database db = new Database("Student");
            db.Command.CommandType = CommandType.StoredProcedure;
            db.Command.CommandText = "tblStudentCommentGetByStudentId";
            db.Command.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            DataTable dt = db.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                StudentComment s = new StudentComment();
                s.Initialize(dr);
                s.InitializeBusiniessData(dr);
                s.IsDirty = false;
                s.IsNew = false;
                s.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(s);
            }
            return this;
        }

        void s_evtIsSavable(bool savable)
        {
            RaiseEvent(savable);
        }

        public StudentCommentList()
        {
            _list = new BindingList<StudentComment>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);
        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new StudentComment();
            ((StudentComment)e.NewObject).evtIsSavable += new IsSavableHandler(s_evtIsSavable);
        }
    }
}
