using Newtonsoft.Json;
using SaglikWebUygulamasi1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaglikWebUygulamasi1.Controllers
{
    [RoutePrefix("api/WebApi/Hastalikbilgileriapi")]
    public class HastalikbilgileriapiController : ApiController
    {
        SaglikDBEntities ent = new SaglikDBEntities();
        String GelenTC;

        [HttpGet]   //Veri çeker
        [Route("GetHastalikBilgisi")]
        public IHttpActionResult GetHastalikBilgisi(String TC)
        {
            List<HastalikBilgisi> model = new List<HastalikBilgisi>();
            //model = ent.Hasta.ToList();
            GelenTC = TC;
            model = ent.HastalikBilgisi.Where(a => a.HastaTC.ToString() == TC).ToList();


            return GetSomeData(model);
        }


        [HttpGet]   //Veri çeker
        [Route("GetSomeData")]
        public IHttpActionResult GetSomeData(List<HastalikBilgisi> model)
        {
            var hastalikBilgisiInfoList = model.Select(hastalikBilgisi => new HastalikBilgisi
            {
                HastaTC = hastalikBilgisi.HastaTC,
                HastalikID = hastalikBilgisi.HastalikID,
                IlacID = hastalikBilgisi.IlacID,
                HastalikTeshisTarihi = hastalikBilgisi.HastalikTeshisTarihi,
                HastalikIyilestigiTarih = hastalikBilgisi.HastalikIyilestigiTarih,
                Hasta = hastalikBilgisi.Hasta,
                Hastalik = hastalikBilgisi.Hastalik,
                Ilac = hastalikBilgisi.Ilac,
            }).ToList();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,  // Güzel bir görünüm için
                NullValueHandling = NullValueHandling.Ignore,  // Null değerleri göz ardı et
                                                               // Başka yapılandırma seçenekleri
            };


            return Json(hastalikBilgisiInfoList, jsonSettings);
        }
    }
}
