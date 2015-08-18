using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseHelper;
using System.Data;
using EmailHelper;

namespace BusinessObjectHelper
{
    public class StudentAddress : HeaderData
    {
        private Guid _studentId;
        private string _street;
        private Guid _zipcodeId;
        private Guid _addressTypeId;
        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _zipcode = string.Empty;

        public Guid StudentId
        {
            get { return _studentId; }
            set { if (_studentId != value) { _studentId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public string Street
        {
            get { return _street; }
            set { { _street = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }
        public Guid AddressTypeId
        {
            get { return _addressTypeId; }
            set { { _addressTypeId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        public Guid ZipcodeId
        {
            get 
            {
                Zipcode zip = new Zipcode();
                zip = zip.GetByZipId(_zipcodeId);
                _city = zip.City;
                _state = zip.State;
                _zipcode = zip.Zip;
                return _zipcodeId; 
            }
            set { { _zipcodeId = value; base.IsDirty = true; RaiseEvent(IsSavable()); } }
        }

        public string City
        {
            get { return _city; }
        }
        public string State
        {
            get { return _state; }
        }
        public string Zipcode
        {
            get { return _zipcode; }
        }
        private Boolean Insert(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentAddressInsert";
                db.Command.Parameters.Clear();
                base.Initialize(db.Command, Guid.Empty);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@street", SqlDbType.VarChar).Value = _street;
                db.Command.Parameters.Add("@addresstypeid", SqlDbType.UniqueIdentifier).Value = _addressTypeId;
                db.Command.Parameters.Add("@zipcodeid", SqlDbType.UniqueIdentifier).Value = _zipcodeId;

                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private Boolean Update(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentAddressUpdate";
                base.Initialize(db.Command, base.Id);
                db.Command.Parameters.Add("@studentid", SqlDbType.UniqueIdentifier).Value = _studentId;
                db.Command.Parameters.Add("@street", SqlDbType.VarChar).Value = _street;
                db.Command.Parameters.Add("@addresstypeid", SqlDbType.UniqueIdentifier).Value = _addressTypeId;
                db.Command.Parameters.Add("@zipcodeid", SqlDbType.UniqueIdentifier).Value = _zipcodeId;
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private Boolean Delete(Database db)
        {
            try
            {
                db.Command.CommandType = CommandType.StoredProcedure;
                db.Command.CommandText = "tblStudentAddressDelete";
                base.Initialize(db.Command, base.Id);
                db.ExecuteNonQueryWithTransaction();
                base.Initialize(db.Command);
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private Boolean IsValid()
        {
            Boolean Result = true;
            try
            {
                if (_street!=null && _street.Trim() == string.Empty)
                    Result = false;
                if (_street!=null &&_street.Length > 50)
                    Result = false;
            }
            catch (Exception ex) { throw; }
            return Result;
        }

        public Boolean IsSavable()
        {
            Boolean Result = true;
            if (base.IsDirty == true && IsValid() == true)
                Result = true;
            else
                Result = false;
            return Result;
        }

        public StudentAddress Save(Database db, Guid parentid)
        {
            _studentId = parentid;
            Boolean result = false;
            if (base.IsNew == true && IsSavable() == true)
                result = Insert(db);
            else if (base.Deleted == true)
                result = Delete(db);
            else if (base.IsNew == false && IsSavable() == true)
                result = Update(db);
            base.IsDirty = false;
            base.IsNew = false;
            return this;
        }

        public Zipcode GetCityStateByZipcode(string zipcode)
        {
            Zipcode zip = new Zipcode();
            zip = zip.GetByZipcode(zipcode);
            return zip;
        }

        public void InitializeBusinessData(DataRow dr)
        {
            _studentId = (Guid)(dr["studentid"]);
            _street = dr["street"].ToString();
            _zipcodeId = (Guid)dr["zipcodeId"];
            _addressTypeId = (Guid)(dr["addresstypeid"]);
        }
    }

}
