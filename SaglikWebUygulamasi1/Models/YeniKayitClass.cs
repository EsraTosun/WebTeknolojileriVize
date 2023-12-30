using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SaglikWebUygulamasi1.Models
{
    public class YeniKayitClass
    {
        public List<Login> loginList { get; set; }
        public List<Hasta> HastaList { get; set; }
    }
}