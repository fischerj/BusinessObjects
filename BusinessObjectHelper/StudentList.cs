using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class StudentList : Event
    {
        #region Private Members
        private BindingList<Student> _list;

        #endregion

        #region Public Properties
        public BindingList<Student> List
        {
            get {return _list ;}
        }
        #endregion

        #region Public Methods
        public StudentList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblStudentGetAll";
            DataTable dt = db.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                Student s = new Student();
                s.Initialize(dr);
                s.InitializeBusinessData(dr);
                s.IsDirty = false;
                s.IsNew = false;
                s.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(s);
            }
            return this;
        }

        public StudentList Save()
        {
            try
            {
                foreach (Student s in _list)
                {
                    if (s.IsSavable() == true)
                        s.Save();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (Student s in _list)
            {
                if (s.IsSavable() == true)
                {
                    result = true;
                    break; //do not continue checking
                }
            }
            return result;
        }
        #endregion

        #region Event Handlers
        void s_evtIsSavable(bool savable)
        {
            RaiseEvent(savable);
        }
        #endregion

        #region Construction
        public StudentList()
        {
            _list = new BindingList<Student>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new Student();
            ((Student)e.NewObject).evtIsSavable 
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
