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
   public class StudentEmailList:Event
    {
        private BindingList<StudentEmail> _list;

        public BindingList<StudentEmail> List
        { get { return _list; } }

        public bool Save(Database db, Guid studentId)
        {
            bool result = true;
            foreach (StudentEmail s in _list)
                if (s.IsSavable() == true)
                {
                    s.Save(db, studentId);
                    if (s.IsNew == true)
                    {
                        result = false;
                        break;
                    }
                }
            return result;

        }

        public bool IsSavable()
        {
            bool result = false;
            
            foreach (StudentEmail c in _list)
                if (c.IsSavable() == true)
                {
                    result = true;
                    break;
                }
            return result;
        }

        public StudentEmailList GetByStudentId(Guid studentId)
        {
            _list.Clear();
            Database db = new Database("Student");
            db.Command.CommandType = CommandType.StoredProcedure;
            db.Command.CommandText = "tblStudentEmailGetByStudentID";
            db.Command.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            DataTable dt = db.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                StudentEmail s = new StudentEmail();
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

        public StudentEmailList()
        {
            _list = new BindingList<StudentEmail>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);
        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new StudentEmail();
            ((StudentEmail)e.NewObject).evtIsSavable += new IsSavableHandler(s_evtIsSavable);
        }
    }
}
