using Newtonsoft.Json;
using SaglikWebUygulamasi1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaglikWebUygulamasi1.Controllers
{
    [RoutePrefix("api/WebApi/Randevuapi")]
    public class RandevuapiController : ApiController
    {
        SaglikDBEntities ent = new SaglikDBEntities();
        String GelenTC;

        [HttpGet]   //Veri çeker
        [Route("GetRandevu")]
        public IHttpActionResult GetRandevu(String TC)
        {
            List<Randevu> model = new List<Randevu>();
            //model = ent.Hasta.ToList();
            GelenTC = TC;
            model = ent.Randevu.Where(a => a.HastaTC.ToString() == TC).ToList();


            return GetSomeData(model);
        }


        [HttpGet]   //Veri çeker
        [Route("GetSomeData")]
        public IHttpActionResult GetSomeData(List<Randevu> model)
        {
            var randevuInfoList = model.Select(randevu => new Randevu
            {
                HastaTC = randevu.HastaTC,
                HastaneBilgisi = randevu.HastaneBilgisi,
                Hasta = randevu.Hasta,
                RandevuSaati = randevu.RandevuSaati,
                RandevuTarihi = randevu.RandevuTarihi,
                HastaneBilgisiID = randevu.HastaneBilgisiID, 
            }).ToList();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,  // Güzel bir görünüm için
                NullValueHandling = NullValueHandling.Ignore,  // Null değerleri göz ardı et
                                                               // Başka yapılandırma seçenekleri
            };

            return Json(randevuInfoList, jsonSettings);
        }
    }
}
