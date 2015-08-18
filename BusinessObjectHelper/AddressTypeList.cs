using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class AddressTypeList : Event
    {
        #region Private Members
        private BindingList<AddressType> _list;

        #endregion

        #region Public Properties
        public BindingList<AddressType> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods
        public AddressTypeList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblAddressTypeGetAll";
            DataTable dt = db.ExecuteQuery();
            AddressType Blank = new AddressType();
            Blank.Type = "Select Address Type";
            _list.Add(Blank);
            foreach (DataRow dr in dt.Rows)
            {
                AddressType e = new AddressType();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsDirty = false;
                e.IsNew = false;
                e.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(e);
            }
            return this;
        }

        public AddressTypeList Save()
        {
            foreach (AddressType e in _list)
            {
                if (e.IsSavable() == true)
                    e.Save();
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (AddressType e in _list)
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
        public AddressTypeList()
        {
            _list = new BindingList<AddressType>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new AddressType();
            ((AddressType)e.NewObject).evtIsSavable
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
