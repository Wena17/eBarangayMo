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
        public IEnumerable<IssuedCert> IssuedCertList()
        {
            var result = new List<IssuedCert>();
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT CERTTYPE.name, CERTTYPE.price, CERTIFICATEREQUEST.unit, PAYMENT.dateOfPayment, (ACCOUNT.FNAME + ' ' + ACCOUNT.MNAME + ' ' + ACCOUNT.LNAME) AS NAME FROM PAYMENT " +
                    "LEFT JOIN  CERTIFICATEREQUEST ON PAYMENT.certRequestID = CERTIFICATEREQUEST.Id AND status = 'P' " +
                    "LEFT JOIN CERTTYPE ON CERTTYPE.Id = CERTIFICATEREQUEST.typeID " +
                    "LEFT JOIN ACCOUNT ON ACCOUNT.OFFICIALID = PAYMENT.officialID ";
                connBrgy.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IssuedCert i = new IssuedCert();
                        i.certName = reader["name"].ToString();
                        i.price = Convert.ToDouble(reader["price"].ToString());
                        i.copies = Convert.ToInt32(reader["unit"].ToString());
                        i.issuedDate = Convert.ToDateTime(reader["dateOfPayment"].ToString());
                        i.officialName = reader["NAME"].ToString();
                        result.Add(i);
                    }
                }
                connBrgy.Close();
                return result;
            }
            catch (Exception)
            {
                return result;
            }
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
                cmd.CommandText = "SELECT Id FROM CERTIFICATEREQUEST WHERE status='N' "; 
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
            catch (Exception)
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
                if (count > 0)
                {
                    cmd.CommandText = "UPDATE CERTIFICATEREQUEST SET status = @status WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", model.requestId);
                    cmd.Parameters.AddWithValue("@status", 'P');
                    connBrgy.Open();
                    int count1 = cmd.ExecuteNonQuery();
                    connBrgy.Close();
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public IEnumerable<Resident> ResidentList()
        {
            var result = new List<Resident>();
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT *, (FNAME + ' ' + MNAME + ' ' + LNAME) as NAME FROM ACCOUNT";
                connBrgy.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Resident res = new Resident();
                        res.id = reader["RESIDENTID"].ToString();
                        res.name = reader["NAME"].ToString();
                        res.bDate = Convert.ToDateTime(reader["BIRTHDATE"].ToString());
                        res.age = Convert.ToInt32(reader["AGE"].ToString());
                        res.civilStat = reader["CIVILSTAT"].ToString();
                        res.vitalStat = reader["VITALSTAT"].ToString();
                        res.email = reader["EMAIL"].ToString();
                        res.phoneNo = reader["PHONENUM"].ToString();
                        result.Add(res);
                    }
                }
                connBrgy.Close();
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public IEnumerable<CertificateRequestModel> MyRequestList(object id)
        {
            var result = new List<CertificateRequestModel>();
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT * FROM CERTIFICATEREQUEST JOIN CERTTYPE ON CERTIFICATEREQUEST.typeID = CERTTYPE.id AND CERTIFICATEREQUEST.requestorID = @userID";
                cmd.Parameters.AddWithValue("@userID", id);
                connBrgy.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CertificateRequestModel req = new CertificateRequestModel();
                        req.name = reader["name"].ToString();
                        req.price = Convert.ToDouble(reader["price"].ToString());
                        req.purpose = reader["purpose"].ToString();
                        req.copies = Convert.ToInt32(reader["unit"].ToString());
                        req.requestDate = Convert.ToDateTime(reader["dateOfRequest"].ToString());
                        result.Add(req);
                    }
                }
                connBrgy.Close();
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public IEnumerable<Document> DocumentList()
        {
            var result = new List<Document>();
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT DOCUMENT.Id, DOCUMENT.filename, DOCUMENT.dateCreated, (ACCOUNT.FNAME + ' ' + ACCOUNT.MNAME + ' ' + ACCOUNT.LNAME) AS NAME FROM DOCUMENT LEFT JOIN ACCOUNT ON DOCUMENT.officialID = ACCOUNT.OFFICIALID";
                connBrgy.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Document d = new Document();
                        d.Id = Convert.ToInt32(reader["Id"].ToString());
                        d.filename = reader["filename"].ToString();
                        d.dateCreated = Convert.ToDateTime( reader["dateCreated"].ToString());
                        d.uploader = reader["NAME"].ToString();
                        result.Add(d);
                    }
                }
                connBrgy.Close();
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public string DocumentFilename(int id)
        {
            string result = null;
            try
            {
                System.Data.SqlClient.SqlCommand cmd = connBrgy.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT filename FROM DOCUMENT WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                connBrgy.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        result = reader["filename"].ToString();
                    } else
                    {
                        result = null;
                    }
                }
                connBrgy.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}