using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBrgyMo4.Controllers
{
    
    public class HomeController : Controller
    {
        string conneBrgyMo = ConfigurationManager.ConnectionStrings["eBrgyMoDB"].ConnectionString;
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
            return View();
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
                using (var db = new SqlConnection(conneBrgyMo))
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
                Response.Write(ex);
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
                using (var db = new SqlConnection(conneBrgyMo))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = cmd.CommandText = "SELECT * FROM [ACCOUNT] WHERE USERNAME = '" + email + "' OR EMAIL = '"+email+"' AND PASS = '" + pass + "'";

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
                Response.Write(ex);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Post()
        {
            var data = new List<object>();
            var post = Request["post"];

            try
            {
                using (var db = new SqlConnection(conneBrgyMo))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " INSERT INTO ACTIVITIES ( POST, NAME ) VALUES( @post, @name )";

                        cmd.Parameters.AddWithValue("@post", post);
                        cmd.Parameters.AddWithValue("@name", Session["LNAME"].ToString() +" , "+ Session["FNAME"].ToString() +" , "+ Session["MNAME"].ToString());

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
                Response.Write(ex);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        
    }
}