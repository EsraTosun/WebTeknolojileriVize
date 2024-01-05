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
    public class HastaneapiController : ApiController
    {
        SaglikDBEntities ent = new SaglikDBEntities();
        String GelenTC;

        [HttpGet]   //Veri çeker
        [Route("GetHastane")]
        public IHttpActionResult GetHastane()
        {
            List<Hastane> model = new List<Hastane>();
            //model = ent.Hasta.ToList();
            model = ent.Hastane.ToList();


            return GetSomeData(model);
        }


        [HttpGet]   //Veri çeker
        [Route("GetSomeData")]
        public IHttpActionResult GetSomeData(List<Hastane> model)
        {
            var tahlilBilgisiInfoList = model.Select(hastane => new Hastane
            {
                HastaneAd = hastane.HastaneAd,
                HastaneAdresi = hastane.HastaneAdresi,
                HastaneBilgisi = hastane.HastaneBilgisi,
                HastaneID = hastane.HastaneID,
                Il = hastane.Il,
                Ilce = hastane.Ilce,
                IlceID = hastane.IlceID,
                IlID = hastane.IlID 
            }).ToList();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,  // Güzel bir görünüm için
                NullValueHandling = NullValueHandling.Ignore,  // Null değerleri göz ardı et
                                                               // Başka yapılandırma seçenekleri
            };

            var json = JsonConvert.SerializeObject(tahlilBilgisiInfoList, Formatting.Indented, jsonSettings);

            return Json(json);
        }
    }
}
