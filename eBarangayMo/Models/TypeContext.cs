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

        public string createCertRequest(CertificateRequestModel model, object residentID)
        {
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO CERTIFICATEREQUEST (purpose, unit, requestorID, typeID) VALUES (@purpose, @copies, @residentId, @requestType)";

                cmd.Parameters.AddWithValue("@purpose", model.purpose);
                cmd.Parameters.AddWithValue("@copies", model.copies);
                cmd.Parameters.AddWithValue("@residentId", residentID);
                cmd.Parameters.AddWithValue("@requestType", model.requestType);
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
        public string DBDocument(Document model, object officialId)
        {
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO DOCUMENT (filename, officialID) VALUES (@filename, @officialId)";

                cmd.Parameters.AddWithValue("@filename", model.filename);
                cmd.Parameters.AddWithValue("@officialId", officialId);
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

        public IEnumerable<string> RequestIdList()
        {
            var result = new List<string>();
            try
            {
                
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT Id FROM CERTIFICATEREQUEST";
                connBrgy.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                { 
                    while (reader.Read())
                    {
                        result.Add(reader["Id"].ToString());
                    }
                }
                connBrgy.Close();
                return result;
            }
            catch (Exception ex)
            {
                // TODO: Notify someone 
                return result;
            }
        }
        public string PaymentLog(Payment model, object officialId)
        {
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO PAYMENT (certRequestID, amount, officialID) VALUES (@requestId, @amount, @officialId)";

                cmd.Parameters.AddWithValue("@requestId", model.requestId);
                cmd.Parameters.AddWithValue("@amount", model.amount);
                cmd.Parameters.AddWithValue("@officialId", officialId);
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