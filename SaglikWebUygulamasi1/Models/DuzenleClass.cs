using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaglikWebUygulamasi1.Models
{
    public class DuzenleClass
    {
        public Hasta duzenlenecekHastaBilgisi { get; set; }
        public Hasta duzenlenecekLoginBilgisi { get; set; }


        public List<Login> loginList { get; set; }
        public List<Hasta> HastaList { get; set; }
    }
}