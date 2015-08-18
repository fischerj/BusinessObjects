using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class ClassList : Event
    {
        #region Private Members
        private BindingList<Class> _list;

        #endregion

        #region Public Properties
        public BindingList<Class> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods
        public ClassList GetByProgramId(Guid programId)
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblClassGetByProgramId";
            db.Command.Parameters.Add("@ProgramId", SqlDbType.UniqueIdentifier).Value = programId;
            DataTable dt = db.ExecuteQuery();
            Class Blank = new Class();
            Blank.Name = "Select Classes";
            _list.Add(Blank);
            foreach (DataRow dr in dt.Rows)
            {
                Class c = new Class();
                c.Initialize(dr);
                c.InitializeBusinessData(dr);
                c.IsDirty = false;
                c.IsNew = false;
                c.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(c);
            }
            return this;
        }

        public ClassList Save()
        {
            foreach (Class p in _list)
            {
                if (p.IsSavable() == true)
                    p.Save();
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (Class p in _list)
            {
                if (p.IsSavable() == true)
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
        public ClassList()
        {
            _list = new BindingList<Class>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new Class();
            ((Class)e.NewObject).evtIsSavable
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
