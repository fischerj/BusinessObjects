using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DatabaseHelper;

namespace BusinessObjectHelper
{
    public class Zipcode
    {
        #region Public Properties

        public Guid Id { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        #endregion

        #region Public Methods
        public Zipcode GetByZipId(Guid zipId)
        {
            try
            {
                Database db = new Database("Student");
                db.Command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Command.CommandText = "tblZipcodeGetById";
                db.Command.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = zipId;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    Id = (Guid)dr["Id"];
                    Zip = dr["Zipcode"].ToString();
                    City = dr["City"].ToString();
                    State = dr["State"].ToString();
                    return this;
                }
                else
                {
                    throw new Exception("Zipcode not found");
                }
            }
            catch
            {
                throw;
            }
        }
        public Zipcode GetByZipcode(string zipcode)
        {
            try
            {
                Database db = new Database("Student");
                db.Command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Command.CommandText = "tblZipcodeGetByZipcode";
                db.Command.Parameters.Add("@Zipcode", System.Data.SqlDbType.VarChar).Value = zipcode;
                DataTable dt = db.ExecuteQuery();
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    Id = (Guid)dr["Id"];
                    Zip = dr["Zipcode"].ToString();
                    City = dr["City"].ToString();
                    State = dr["State"].ToString();
                    return this;
                }
                else
                {
                    throw new Exception("Zipcode not found");
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
