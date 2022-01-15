using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBarangayMo.Controllers
{
    public class HomeController : Controller
    {
        //string conneBrgyDB = ConfigurationManager.ConnectionStrings["eBrgyDBF"].ConnectionString;
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
        public ActionResult CreateAcc()
        {
            return View();
        }
        public ActionResult LoginUser()
        {
            return View();
        }

        //public ActionResult Account()
        //{
        //    var data = new List<object>();
        //    var lname = Request["lastname"];
        //    var fname = Request["firstname"];
        //    var mname = Request["middlename"];
        //    var age = Request["age"];
        //    var birthdate = Request["bdate"];
        //    var role = Request["role"];
        //    var civilStat = Request["civilStat"];
        //    string lifeStat = "Alive";
        //    var email = Request["email"];
        //    var pass = Request["password"];
        //    var famCode = Request["famCode"];
        //    var officialNotifID = Request["officialNotifID"];
        //    var residentNotifID = Request["residentNotifID"];

        //    try
        //    {
        //        using (var db = new SqlConnection(conneBrgyDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = " INSERT INTO [USER] ( LNAME, FNAME, MNAME, AGE, BIRTHDATE, ROLE, CIVILSTAT, LIFESTAT, EMAIL, PASS, FAMCODE, OFFICIALNOTIFID, RESIDENTNOTIFID ) VALUES( @lastname, @firstname, @middlename, @age, @birthdate, @role, @civilStat, @lifeStat, @email, @pass, @famCode, @officialNotifID, @residentNotifID)";

        //                cmd.Parameters.AddWithValue("@lastname", lname);
        //                cmd.Parameters.AddWithValue("@firstname", fname);
        //                cmd.Parameters.AddWithValue("@middlename", mname);
        //                cmd.Parameters.AddWithValue("@age", age);
        //                cmd.Parameters.AddWithValue("@birthdate", birthdate);
        //                cmd.Parameters.AddWithValue("@role", role);
        //                cmd.Parameters.AddWithValue("@civilStat", civilStat);
        //                cmd.Parameters.AddWithValue("@lifeStat", lifeStat);
        //                cmd.Parameters.AddWithValue("@email", email);
        //                cmd.Parameters.AddWithValue("@pass", pass);
        //                cmd.Parameters.AddWithValue("@famCode", famCode);
        //                cmd.Parameters.AddWithValue("@officialNotifID", officialNotifID);
        //                cmd.Parameters.AddWithValue("@residentNotifID", residentNotifID);
        //                var ctr = cmd.ExecuteNonQuery();

        //                if (ctr >= 1)
        //                {
        //                    data.Add(new
        //                    {
        //                        mess = 1,
        //                    });
        //                }


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex);
        //    }
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult Login()
        //{
        //    var data = new List<object>();
        //    var email = Request["email"];
        //    var pass = Request["password"];
        //    var flag = 0;

        //    try
        //    {
        //        using (var db = new SqlConnection(conneBrgyDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = cmd.CommandText = "SELECT * FROM [USER] WHERE EMAIL = '" + email + "' AND PASS = '" + pass + "'";

        //                cmd.Parameters.AddWithValue("@email", email);
        //                cmd.Parameters.AddWithValue("@pass", pass);
        //                SqlDataReader rdr = cmd.ExecuteReader();

        //                if (rdr.Read())
        //                {
        //                    //Session["email"] = email;
        //                    //Session["password"] = pass;
        //                    //Session["name"] = rdr["FNAME"];
        //                    //Session["balance"] = rdr["BALANCE"];
        //                    flag = 1;
        //                }
        //                data.Add(new
        //                {
        //                    mess = flag
        //                });



        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex);
        //    }
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
    }
}