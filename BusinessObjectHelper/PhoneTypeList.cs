using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class PhoneTypeList : Event
    {
        #region Private Members
        private BindingList<PhoneType> _list;

        #endregion

        #region Public Properties
        public BindingList<PhoneType> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods
        public PhoneTypeList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblPhoneTypeGetAll";
            DataTable dt = db.ExecuteQuery();
            PhoneType Blank = new PhoneType();
            Blank.Type = "Select Phone Type";
            _list.Add(Blank);
            foreach (DataRow dr in dt.Rows)
            {
                PhoneType e = new PhoneType();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsDirty = false;
                e.IsNew = false;
                e.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(e);
            }
            return this;
        }

        public PhoneTypeList Save()
        {
            foreach (PhoneType e in _list)
            {
                if (e.IsSavable() == true)
                    e.Save();
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (PhoneType e in _list)
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
        public PhoneTypeList()
        {
            _list = new BindingList<PhoneType>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new PhoneType();
            ((PhoneType)e.NewObject).evtIsSavable
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
