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
    public class StudentClassList : Event
    {
        private BindingList<StudentClass> _list;

        public BindingList<StudentClass> List
        { get { return _list; } }

        public bool Save(Database db, Guid studentId)
        {
            bool result = true;
            try
            {

                foreach (StudentClass s in _list)
                    if (s.IsSavable() == true)
                    {
                        s.Save(db, studentId);
                        if (s.IsNew == true)
                        {
                            result = false;
                            break;
                        }
                    }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public bool IsSavable()
        {
            bool result = false;

            foreach (StudentClass c in _list)
                if (c.IsSavable() == true)
                {
                    result = true;
                    break;
                }
            return result;
        }

        public StudentClassList GetByStudentId(Guid studentId)
        {
            _list.Clear();
            Database db = new Database("Student");
            db.Command.CommandType = CommandType.StoredProcedure;
            db.Command.CommandText = "tblStudentClassGetByStudentID";
            db.Command.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            DataTable dt = db.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                StudentClass s = new StudentClass();
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

        public StudentClassList()
        {
            _list = new BindingList<StudentClass>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);
        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new StudentClass();
            ((StudentClass)e.NewObject).evtIsSavable += new IsSavableHandler(s_evtIsSavable);
        }
    }
}
