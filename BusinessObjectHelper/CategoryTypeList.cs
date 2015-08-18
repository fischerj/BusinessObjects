using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class CategoryTypeList : Event
    {
        #region Private Members
        private BindingList<CategoryType> _list;

        #endregion

        #region Public Properties
        public BindingList<CategoryType> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods
        public CategoryTypeList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblCategoryGetAll";
            DataTable dt = db.ExecuteQuery();
            CategoryType Blank = new CategoryType();
            Blank.Type = "Select Catetory Type";
            _list.Add(Blank);
            foreach (DataRow dr in dt.Rows)
            {
                CategoryType e = new CategoryType();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsDirty = false;
                e.IsNew = false;
                e.evtIsSavable += new IsSavableHandler(s_evtIsSavable);
                _list.Add(e);
            }
            return this;
        }

        public CategoryTypeList Save()
        {
            foreach (CategoryType e in _list)
            {
                if (e.IsSavable() == true)
                    e.Save();
            }
            return this;
        }

        public Boolean IsSavable()
        {
            Boolean result = false;
            foreach (CategoryType e in _list)
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
        public CategoryTypeList()
        {
            _list = new BindingList<CategoryType>();
            _list.AddingNew += new AddingNewEventHandler(_list_AddingNew);

        }

        void _list_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new CategoryType();
            ((CategoryType)e.NewObject).evtIsSavable
                += new IsSavableHandler(s_evtIsSavable);

        }
        #endregion

    }
}
