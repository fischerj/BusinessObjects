using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class EmailTypeList : Event
    {
        #region Private Members
        private BindingList<EmailType> _list;

        #endregion

        #region Public Properties
        public BindingList<EmailType> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods
        public EmailTypeList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblEmailTypeGetAll";
            DataTable dt = db.ExecuteQuery();
            EmailType Blank = new EmailType();
            Blank.Type = "Select Phone Type";
            _list.Add(Blank);
            foreach (DataRow dr in dt.Rows)
            {
                EmailType e = new EmailType();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsDirty = false;
                e.IsNew = false;
                e.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(e);
            }
            return this;
        }

        public EmailTypeList Save()
        {
            foreach (EmailType e in _list)
            {
                if (e.IsSavable() == true)
                    e.Save();
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (EmailType e in _list)
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
        public EmailTypeList()
        {
            _list = new BindingList<EmailType>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new EmailType();
            ((EmailType)e.NewObject).evtIsSavable
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
