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
    public class TahilibilgisiapiController : ApiController
    {

        SaglikDBEntities ent = new SaglikDBEntities();
        String GelenTC;

        [HttpGet]   //Veri çeker
        [Route("GetTahlilBilgisi")]
        public IHttpActionResult GetTahlilBilgisi(String TC)
        {
            List<TahlilBilgisi> model = new List<TahlilBilgisi>();
            //model = ent.Hasta.ToList();
            GelenTC = TC;
            model = ent.TahlilBilgisi.Where(a => a.HastaTC.ToString() == TC).ToList();


            return GetSomeData(model);
        }


        [HttpGet]   //Veri çeker
        [Route("GetSomeData")]
        public IHttpActionResult GetSomeData(List<TahlilBilgisi> model)
        {
            var tahlilBilgisiInfoList = model.Select(tahlilBilgisi => new TahlilBilgisi
            {
                HastaTC = tahlilBilgisi.HastaTC,
                A = tahlilBilgisi.A,
                TahlilTarihi = tahlilBilgisi.TahlilTarihi,
                B12 = tahlilBilgisi.B12,
                Glukoz = tahlilBilgisi.Glukoz,
                Hemoglobin = tahlilBilgisi.Hemoglobin,
                Kalsiyum = tahlilBilgisi.Kalsiyum,
                KanBasıncı = tahlilBilgisi.KanBasıncı,
                Kolestrol = tahlilBilgisi.Kolestrol,
                Hasta = tahlilBilgisi .Hasta,
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
