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
    public class StudentAddressList : Event
    {
        private BindingList<StudentAddress> _list;

        public BindingList<StudentAddress> List
        { get { return _list; } }

        public bool Save(Database db, Guid studentId)
        {
            bool result = true;
            foreach (StudentAddress s in _list)
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

            foreach (StudentAddress c in _list)
                if (c.IsSavable() == true)
                {
                    result = true;
                    break;
                }
            return result;
        }

        public StudentAddressList GetByStudentId(Guid studentId)
        {
            _list.Clear();
            Database db = new Database("Student");
            db.Command.CommandType = CommandType.StoredProcedure;
            db.Command.CommandText = "tblStudentAddressGetByStudentID";
            db.Command.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            DataTable dt = db.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                StudentAddress s = new StudentAddress();
                s.Initialize(dr);
                s.InitializeBusinessData(dr);
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

        public StudentAddressList()
        {
            _list = new BindingList<StudentAddress>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);
        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new StudentAddress();
            ((StudentAddress)e.NewObject).evtIsSavable += new IsSavableHandler(s_evtIsSavable);
        }
    }
}
