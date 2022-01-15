using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper; 

namespace eBarangayMo.Models
{
    public class TypeContext
    {
        SqlConnection connBrgy = new SqlConnection (ConfigurationManager.ConnectionStrings["eBarangayMoDBFCONN"].ToString());
        public IEnumerable<CertificateRequestModel> GetTypeList()
        {
            string query = "SELECT [Id],[name], [price] FROM [dbo].[CERTTYPE]";
            var result = connBrgy.Query<CertificateRequestModel>(query);
            return result;
        }

        public string createCertRequest(CertificateRequestModel model)
        {
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT CERTIFICATEREQUEST (purpose, unit, requestorID, typeID) VALUES ('" + model.purpose + "'," + model.copies + ",5," + model.requestType + ")"; // TODO Use proper questy parameters. Add requestor ID from session.
                connBrgy.Open();
                int count = cmd.ExecuteNonQuery();
                connBrgy.Close();
                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}