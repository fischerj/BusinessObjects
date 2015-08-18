using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using EmailHelper;
namespace BusinessObjectHelper
{
    public class Student : HeaderData
    {
        #region Private Members
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private string _UserName = string.Empty;
        private string _Password = string.Empty;
        private Guid _ProgramId = Guid.Empty;

        private StudentCommentList _comments = null;
        private StudentEmailList _emails = null;
        private StudentPhoneList _phones = null;
        private StudentAddressList _addresses = null;
        private StudentClassList _classes = null;

        #endregion

        #region Public Properties
        public string FirstName
        {
            get 
            { 
                return _FirstName;
            }
            set 
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    base.IsDirty = true;
                    Boolean savable = IsSavable();
                    RaiseEvent(savable);

                }
            }
        }
        public string LastName
        {
            get { return _LastName;}
            set 
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    base.IsDirty = true;
                    RaiseEvent(IsSavable());
                }
            }
        }

        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    base.IsDirty = true;
                    RaiseEvent(IsSavable());
                }
            }
        }

        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    base.IsDirty = true;
                    RaiseEvent(IsSavable());
                }
            }
        }

        public Guid ProgramId
        {
            get
            {
                return _ProgramId;
            }
            set
            {
                if (_ProgramId != value)
                {
                    base.IsDirty = true;
                    RaiseEvent(IsSavable());
                    _ProgramId = value;
                }
            }
        }

        public StudentCommentList Comments
        {
            get 
            {
                if (_comments == null)
                {
                    _comments = new StudentCommentList();
                    _comments = _comments.GetByStudentId(base.Id);
                }
                return _comments;
            }
        }

        public StudentEmailList Emails
        {
            get
            {
                if (_emails == null)
                {
                    _emails = new StudentEmailList();
                    _emails = _emails.GetByStudentId(base.Id);
                }

                return _emails;
            }
        }
        public StudentPhoneList Phones
        {
            get
            {
                if (_phones == null)
                {
                    _phones = new StudentPhoneList();
                    _phones = _phones.GetByStudentId(base.Id);
                }

                return _phones;
            }
        }

        public StudentAddressList Addresses
        {
            get
            {
                if (_addresses == null)
                {
                    _addresses = new StudentAddressList();
                    _addresses = _addresses.GetByStudentId(base.Id);
                }

                return _addresses;
            }
        }

        public StudentClassList Classes
        {
            get
            {
                if (_classes == null)
                {
                    _classes = new StudentClassList();
                    _classes = _classes.GetByStudentId(base.Id);
                }

                return _classes;
            }
        }
        #endregion

        #region Private Methods
        private Boolean Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentInsert";
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = _FirstName;
                db.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = _LastName;
                db.Command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = _UserName;
                db.Command.Parameters.Add("@Password", SqlDbType.VarChar).Value = _Password;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        private Boolean Update(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentUpdate";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = _FirstName;
                db.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = _LastName;
                db.Command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = _UserName;
                db.Command.Parameters.Add("@Password", SqlDbType.VarChar).Value = _Password;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Boolean Delete(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentDelete";
                base.Initialize(db.Command, base.Id);
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Boolean IsValid()
        {
            Boolean Result = true;
            if (_FirstName.Trim() == string.Empty)
            {
                Result = false;
            }
            if (_LastName.Trim() == string.Empty)
            {
                Result = false;
            }
            if (_FirstName.Trim().Length > 50)
            {
                Result = false;
            }
            if (_LastName.Trim().Length > 50)
            {
                Result = false;
            }

            if (_UserName.Trim() == string.Empty)
            {
                Result = false;
            }
            if (_Password.Trim() == string.Empty)
            {
                Result = false;
            }
            if (_UserName.Trim().Length > 50)
            {
                Result = false;
            }
            if (_Password.Trim().Length > 50)
            {
                Result = false;
            }
            return Result;
        }

        #endregion

        #region Public Methods
        public Boolean IsSavable()
        {
            Boolean Result = true;
            if (base.IsDirty == true && IsValid() == true)
                Result = true;
            else
                Result = false;

            return Result;
        }

        public Student Save()
        {
            Database db = new Database("student");
            try
            {
                
                db.BeginTransaction();

                Boolean result = true;
                if (base.IsNew == true && IsSavable() == true)
                {
                    result = Insert(db);
                }
                else if (base.Deleted == true)
                {
                    result = Delete(db);
                }
                else if (base.IsNew == false && IsSavable() == true)
                {
                    result = Update(db);
                }
                base.IsDirty = false;
                base.IsNew = false;

                //SAVE THE CHILDREN
                if (result == true && _comments != null && _comments.IsSavable() == true)
                {
                    result = _comments.Save(db, base.Id);
                }

                if (result == true && _emails != null && _emails.IsSavable() == true)
                {
                    result = _emails.Save(db, base.Id);
                }
                if (result == true && _phones != null && _phones.IsSavable() == true)
                {
                    result = _phones.Save(db, base.Id);
                }
                if (result == true && _addresses != null && _addresses.IsSavable() == true)
                {
                    result = _addresses.Save(db, base.Id);
                }
                if (result == true && _classes != null && _classes.IsSavable() == true)
                {
                    result = _classes.Save(db, base.Id);
                }
                if (result == true)
                {
                    db.EndTransaction();
                }
                else
                {
                    db.RollBackTransaction();
                }
            }
            catch (Exception e)
            {
                db.RollBackTransaction();
                throw;
            }
            return this;
        }

        public Student Login(string username, string password)
        {
            Database db = new Database("student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentLogin";
                db.Command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = username;
                db.Command.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    base.Initialize(dr);
                    InitializeBusinessData(dr);
                    return this;
                }
                else
                    throw new ApplicationException("Invalid Login");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Student Register(string firstname,
                                string lastname,
                                string username, 
                                string password,
                                Guid programId)
        {
            Database db = new Database("student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentRegister";
                db.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = firstname;
                db.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = lastname;
                db.Command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = username;
                db.Command.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;
                db.Command.Parameters.Add("@ProgramId", SqlDbType.UniqueIdentifier).Value = programId;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    base.Initialize(dr);
                    InitializeBusinessData(dr);
                    return this;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Boolean Recover(string firstname,
                               string lastname,
                               string username,
                               string emailaddress)
        {
            Database db = new Database("student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentRecover";
                db.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = firstname;
                db.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = lastname;
                db.Command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = username;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    Email email = new Email();
                    email.Host = "smtp.gmail.com";
                    email.Port = 587;
                    email.Username = "jaydavidfischer@gmail.com";
                    email.Password = "fischerj12";
                    email.To = emailaddress;
                    email.From = "jaydavidfischer@gmail.com";
                    email.Subject = "Password Recovery";
                    email.Body = 
                        String.Format("Your password is {0}", 
                        dr["password"].ToString());
                    email.Send();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }            
        }

        public Student ChangePassword(string newpassword)
        {
            Database db = new Database("student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentChangePassword";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@Password", SqlDbType.VarChar).Value = newpassword;
                db.ExecuteNonQuery();
                base.Initialize(db.Command);
                return this;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Student GetById(Guid Id)
        {
            Database db = new Database("student");
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentGetById";
                db.Command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    base.Initialize(dr);
                    InitializeBusinessData(dr);
                    return this;
                }
                else
                    throw new Exception("Student not found");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void InitializeBusinessData(DataRow dr)
        {
            _FirstName = dr["FirstName"].ToString();
            _LastName = dr["LastName"].ToString();
            _UserName = dr["UserName"].ToString();
            _Password = dr["Password"].ToString();
            _ProgramId = new Guid(dr["programId"].ToString());
        }

        #endregion

    }

}
