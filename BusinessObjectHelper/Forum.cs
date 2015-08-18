using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
namespace BusinessObjectHelper
{
    public class Forum 
    {
        #region Private Members
        private object _id = 0;
        private object _parentId = 0;
        private string _message = string.Empty;
        #endregion

        #region Public Properties
        public object Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public object ParentId
        {
            get
            {
                return _parentId;
            }
            set
            {
                _parentId = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                }
            }
        }

        #endregion


        #region Public Methods
        public void InitializeBusinessData(DataRow dr)
        {
            _id = (object)dr["Id"];

            if (dr["ParentId"]==DBNull.Value)
                _parentId=null;
            else _parentId =(object)dr["ParentId"];

            _message = dr["Message"].ToString();
        }

        #endregion


    }
}

