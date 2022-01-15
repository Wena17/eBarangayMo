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
                    return Redirect("/CertRequests"); //TODO: Return a page that shows all the request of the specific user
                } 
            }
                ViewBag.VBtypelist = new SelectList(typeList, "id", "name");
                return View(model);
        }
        public ActionResult CertRequests()
        {
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

    }


}