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
        public DateTime requestDate { get; set; }
        public SelectList TypeList { get; set; }
        public string msg { get; set;  }      
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
    }
    public class Document
    {
        public  int Id { get; set; }
        public string filename { get; set; }
        public DateTime dateCreated { get; set; }
        public string uploader { get; set; }
        public string msg { get; set; }    }
    public class Payment
    {
        public int requestId { get; set; }
        public double amount { get; set; }
        public string msg { get; set; }
    }

    public class IssuedCert
    {
        public string certName { get; set; }
        public double price { get; set; }
        public int copies { get; set; }
        public DateTime issuedDate { get; set; }
        public string officialName { get; set; }
        public string residentName { get; set; }
    }
}