using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBarangayMo.Models
{
    public class CertificateRequestModel
    {
        public int id { get; set; }
        public int requestType { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string purpose { get; set; }
        public int copies { get; set; }        
        public SelectList TypeList { get; set; }
        public string msg { get; set;  }


      
    }
    public class CertificateRequests
    {
        public string name { get; set; }
        public double price { get; set; }
        public string purpose { get; set; }
        public int copies { get; set; }
        public DateTime requestDate { get; set; }
        public List<CertificateRequests> requests { get; set; }
    }
    public class Resident
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime bDate { get; set; }
        public int age { get; set; }
        public string civilStat { get; set; }
        public string vitalStat { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public List<Resident> residentsList { get; set; }
    }
    public class Document
    {
        public string filename { get; set; }
        public string msg { get; set; }
    }
}