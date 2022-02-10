using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBarangayMo.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace eBarangayMo.Controllers
{
    public class HomeController : Controller
    {
        string connBrgy = ConfigurationManager.ConnectionStrings["eBarangayMoDBFCONN"].ConnectionString;


        TypeContext tcon = new TypeContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult IssuedCert()
        {
            ViewBag.IssuedCertList = tcon.IssuedCertList();
            return View();
        }
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
                model.msg = tcon.createCertRequest(model, Session["residentID"]);
                if (model.msg == null)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Request Succesfully Sent! Visit the Barangay hall to pay and fetch your Certificate');window.location.href='/Home/MyRequests';</script>");
                } 
            }
                ViewBag.VBtypelist = new SelectList(typeList, "id", "name");
                return View(model);
        }
        public ActionResult MyRequests()
        {
            ViewBag.MyRequestList = tcon.MyRequestList(Session["residentID"]);
            return View();
        }
        public ActionResult brgyOffPage()
        {
            return View();
        }
        public ActionResult fileComplaint()
        {
            var data = new List<object>();
            var complaint = Request["complaint"];
            var proof = Request["proof"];
            var witness = Request["witness"];
            var date = Request["date"];

            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " INSERT INTO COMPLAINT ( RESIDENTID, NAME, DATE, COMPLAINT, PROOF, WITNESS ) VALUES( @residentid, @name, @date, @complaint, @proof, @witness )";

                        cmd.Parameters.AddWithValue("@residentid", Session["RESIDENTID"]);
                        cmd.Parameters.AddWithValue("@name", Session["LNAME"].ToString() + " , " + Session["FNAME"].ToString());
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@complaint", complaint);
                        cmd.Parameters.AddWithValue("@proof", proof);
                        cmd.Parameters.AddWithValue("@witness", witness);
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


        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session["email"] = null;
            Session["username"] = null;
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
            DateTime dob = Convert.ToDateTime(birthdate);
            var age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
            {
                age = age - 1;
            }
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
                Response.Write(ex);
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Login()
        {
            var data = new List<object>();
            var username = Request["email"];
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
                        cmd.CommandText = cmd.CommandText = "SELECT * FROM [ACCOUNT] WHERE USERNAME = '" + @username + "' OR EMAIL = '" + @username + "' AND PASS = '" + @pass + "'";

                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@pass", pass);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            Session["username"] = username;
                            Session["password"] = pass;
                            Session["lname"] = rdr["LNAME"];
                            Session["fname"] = rdr["FNAME"];
                            Session["mname"] = rdr["MNAME"];
                            Session["residentID"] = rdr["RESIDENTID"].ToString();
                            var offID = rdr["OFFICIALID"];
                            if (offID != System.DBNull.Value)
                            {
                                Session["officialID"] = offID;
                            }
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
            var datetime = Request["dateTime"];

            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " INSERT INTO ACTIVITIES ( POST, NAME, DATETIME ) VALUES( @post, @name, @datetime )";

                        cmd.Parameters.AddWithValue("@post", post);
                        cmd.Parameters.AddWithValue("@name", Session["LNAME"].ToString() + " , " + Session["FNAME"].ToString());
                        cmd.Parameters.AddWithValue("@datetime", datetime);
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
        public ActionResult displayNaU()
        {
            var data = new List<object>();
            try
            {
                using (var db = new SqlConnection(connBrgy))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ACTIVITIES ORDER BY DATETIME DESC";
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            data.Add(new
                            {
                                post = reader["POST"].ToString(),
                                name = reader["NAME"].ToString(),
                                datetime = reader["DATETIME"].ToString(),
                                //time = reader["TIME"].ToString(),
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

        public ActionResult residentPage()
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/");
            }

            return View();
        } 


        [HttpGet]
        public ActionResult DocumentUploading()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DocumentUploading(HttpPostedFileBase file)
        {
            Document model = new Document();
            string saveDIR = Server.MapPath("/UploadedFile");
            try
            {
                if (file.ContentLength > 0)
                {
                    string filename = Server.HtmlEncode(file.FileName);
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (System.IO.File.Exists(Path.Combine(saveDIR, filename))) 
                    {
                        ViewBag.Message = "File already exist";
                        return View();
                    }
                    else
                    {
                        model.filename = filename;
                        string savePath = Path.Combine(saveDIR, filename);
                        if (model.filename.Length > 0)
                        {
                            
                            model.msg = tcon.DBDocument(model, Session["officialID"]);
                            file.SaveAs(savePath);
                            if (model.msg == null)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Your file was uploaded successfully.');window.location.href='/Home/DocumentList';</script>");
                            }
                        }
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Upload Failed: Try again";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Upload Failed: Try again";
                return View();
            }
        }


        public ActionResult Residents()
        {
            ViewBag.ResidentList = tcon.ResidentList();
            return View();
        }

        public ActionResult Payment()
        {
            ViewBag.RequestList = tcon.RequestList();
            return View();
        }

        [HttpPost]
        public ActionResult Payment(string selectedRequestId, string amountPaid)
        {
            Payment model = new Payment();
            model.requestId = Convert.ToInt32(selectedRequestId);
            model.amount = Convert.ToDouble(amountPaid);
            if(selectedRequestId != null && amountPaid != null)
            {
                model.msg = tcon.PaymentLog(model, Session["officialID"]);
                if(model.msg == null)
                {
                    Session["message"] = "Payment succeded";
                    return RedirectToAction("IssuedCert"); 
                }
            }
            Session["message"] = "Payment failed!";
            return Redirect("/Home/Payment");
        }

        public ActionResult DocumentList()
        {
            ViewBag.DocumentList = tcon.DocumentList();
            return View();
        }

        public ActionResult Documents(int id)
        {
            string filename = tcon.DocumentFilename(id);
            if (filename == null)
            {
                Session["message"] = "File not found";
                return Redirect("/Home/DocumentList");
            }
            string ext = System.IO.Path.GetExtension(filename).ToLower();
            ViewBag.documentUrl = "/UploadedFile/" + filename;
            ViewBag.mimeType = MimeMapping.GetMimeMapping(ext);
            return View();
        }

    }


}