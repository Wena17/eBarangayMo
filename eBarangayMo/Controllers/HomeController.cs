using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBarangayMo.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace eBarangayMo.Controllers
{
    public class HomeController : Controller
    {
        string connBrgy = ConfigurationManager.ConnectionStrings["eBarangayMoDBFCONN"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult IssuedCert()
        {
            return View();
        }

        TypeContext tcon = new TypeContext();
        public ActionResult CertRequest()
        {
            CertificateRequestModel model = new CertificateRequestModel();
            model.msg = "";
            model.requestType = 0;
            model.copies = 1;
            model.purpose = "";
            ViewBag.VBtypelist = new SelectList(tcon.GetTypeList(), "id", "name");
            return View(model);
        }

        [HttpPost]
        public ActionResult CertRequest(string type, string purpose, string copies)
        {
            CertificateRequestModel model = new CertificateRequestModel();
            var typeList = tcon.GetTypeList();
            if (type.Length > 0)
            {
                model.requestType = Convert.ToInt32(type);
                CertificateRequestModel selectedType = typeList.FirstOrDefault(entry => model.requestType == entry.id);
                model.price = selectedType.price;
            }
            model.purpose = purpose;
            model.copies = copies.Length > 0 ? Convert.ToInt32(copies) : 1;
            if (purpose.Length > 0 && model.requestType > 0)
            {
                model.msg = tcon.createCertRequest(model);
                if (model.msg == null)
                {
                    return Redirect("/Home/CertRequests"); 
                } 
            }
                ViewBag.VBtypelist = new SelectList(typeList, "id", "name");
                return View(model);
        }
        public ActionResult CertRequests()
        {
            //TODO: Return a page that shows all the request of the specific user
            CertificateRequests req = new CertificateRequests();
            DataSet ds = new DataSet();
            List<CertificateRequests> requestsList = new List<CertificateRequests>();

            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        db.Open();
                    }
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM CERTIFICATEREQUEST JOIN CERTTYPE ON CERTIFICATEREQUEST.typeID = CERTTYPE.id AND CERTIFICATEREQUEST.requestorID = @userID";
                        cmd.Parameters.AddWithValue("@userID", Session["residentID"]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            CertificateRequests requestobj = new CertificateRequests();
                            requestobj.name = ds.Tables[0].Rows[i]["name"].ToString();
                            requestobj.price = Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString());
                            requestobj.purpose = ds.Tables[0].Rows[i]["purpose"].ToString();
                            requestobj.copies = Convert.ToInt32(ds.Tables[0].Rows[i]["unit"].ToString());
                            requestobj.requestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["dateOfRequest"].ToString());
                            requestsList.Add(requestobj);
                        }
                        req.requests = requestsList;
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert(' " + ex.ToString() + " ')</script>");
            }

            return View(requestsList);
        }
        public ActionResult brgyOffPage()
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session["email"] = null;
            Session["password"] = null;
            Session.Clear();
            Response.Redirect("~/");
            return View();
        }


        public ActionResult User()
        {
            var data = new List<object>();
            var lname = Request["lastname"];
            var fname = Request["firstname"];
            var mname = Request["middlename"];
            var age = Request["age"];
            var birthdate = Request["bdate"];
            var civilStat = Request["civilStat"];
            string lifeStat = "Alive";
            var email = Request["mail"];
            var pass = Request["pass"];
            var residentID = Request["residentID"];
            var phoneNum = Request["phonenum"];
            var role = Request["role"];
            var officialID = Request["officialID"];
            var username = Request["username"];

            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " INSERT INTO [ACCOUNT] ( LNAME, FNAME, MNAME, AGE, BIRTHDATE, CIVILSTAT, VITALSTAT, EMAIL, PASS, PHONENUM, RESIDENTID, ROLE, OFFICIALID, USERNAME) VALUES( @lastname, @firstname, @middlename, @age, @birthdate, @civilStat, @vitalStat, @email, @pass, @phoneNum, @residentID, @role, @officialID, @username)";

                        cmd.Parameters.AddWithValue("@lastname", lname);
                        cmd.Parameters.AddWithValue("@firstname", fname);
                        cmd.Parameters.AddWithValue("@middlename", mname);
                        cmd.Parameters.AddWithValue("@age", age);
                        cmd.Parameters.AddWithValue("@birthdate", birthdate);
                        cmd.Parameters.AddWithValue("@civilStat", civilStat);
                        cmd.Parameters.AddWithValue("@vitalStat", lifeStat);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@pass", pass);
                        cmd.Parameters.AddWithValue("@phoneNum", phoneNum);
                        cmd.Parameters.AddWithValue("@residentID", residentID);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@officialID", officialID);
                        cmd.Parameters.AddWithValue("@username", username);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Session["password"] = pass;
                            data.Add(new
                            {
                                mess = 1,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            var data = new List<object>();
            var email = Request["email"];
            var pass = Request["password"];
            var flag = 0;

            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = cmd.CommandText = "SELECT * FROM [ACCOUNT] WHERE USERNAME = '" + email + "' OR EMAIL = '" + email + "' AND PASS = '" + pass + "'";

                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@pass", pass);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            Session["email"] = email;
                            Session["password"] = pass;
                            Session["lname"] = rdr["LNAME"];
                            Session["fname"] = rdr["FNAME"];
                            Session["mname"] = rdr["MNAME"];
                            Session["residentID"] = rdr["RESIDENTID"];
                            flag = 1;
                        }
                        data.Add(new
                        {
                            mess = flag
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Post()
        {
            var data = new List<object>();
            var post = Request["post"];

            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " INSERT INTO ACTIVITIES ( POST, NAME ) VALUES( @post, @name )";

                        cmd.Parameters.AddWithValue("@post", post);
                        cmd.Parameters.AddWithValue("@name", Session["LNAME"].ToString() + " , " + Session["FNAME"].ToString() + " , " + Session["MNAME"].ToString());

                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr >= 1)
                        {
                            data.Add(new
                            {
                                mess = 1,
                            });
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }


}