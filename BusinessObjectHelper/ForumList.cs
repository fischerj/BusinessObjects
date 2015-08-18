using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using System.ComponentModel;

namespace BusinessObjectHelper
{
    public class ForumList
    {
        #region Private Members
        private List<Forum> _list= new List<Forum>();

        #endregion

        #region Public Properties
        public List<Forum> List
        {
            get { return _list; }
        }
        #endregion

        #region Public Methods

        public ForumList GetAll()
        {
            _list.Clear();
            Database db = new Database("student");
            db.Command.CommandType = System.Data.CommandType.StoredProcedure;
            db.Command.CommandText = "tblForumGetAll";
            DataTable dt = db.ExecuteQuery();
            
            foreach (DataRow dr in dt.Rows)
            {
                Forum f = new Forum();
                f.InitializeBusinessData(dr);
                _list.Add(f);
            }
            return this;
        }


        List<Forum> forumList = new List<Forum>();
        public List<Forum> Recurse(object parentId)
        {
            foreach (Forum f in _list)
            {
                forumList.Add(f);
                if (f.ParentId != null)
                {
                    Recurse(f.ParentId);
                }

            }




            return forumList;
        }
        #endregion


    }
}
