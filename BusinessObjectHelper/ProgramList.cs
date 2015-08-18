using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class ProgramList : Event
    {
        #region Private Members
        private BindingList<Program> _list;

        #endregion

        #region Public Properties
        public BindingList<Program> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods
        public ProgramList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblProgramGetAll";
            DataTable dt = db.ExecuteQuery();
            Program Blank = new Program();
            Blank.Name = "Select Program";
            _list.Add(Blank);
            foreach (DataRow dr in dt.Rows)
            {
                Program p = new Program();
                p.Initialize(dr);
                p.InitializeBusinessData(dr);
                p.IsDirty = false;
                p.IsNew = false;
                p.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(p);
            }
            return this;
        }

        public ProgramList Save()
        {
            foreach (Program p in _list)
            {
                if (p.IsSavable() == true)
                    p.Save();
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (Program e in _list)
            {
                if (e.IsSavable() == true)
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
        public ProgramList()
        {
            _list = new BindingList<Program>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new Program();
            ((Program)e.NewObject).evtIsSavable
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
