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
    [RoutePrefix("api/WebApi/Hastabilgileriapi")]
    public class HastabilgileriapiController : ApiController
    {
        SaglikDBEntities ent = new SaglikDBEntities();
        String GelenTC;

        [HttpGet]   //Veri çeker
        [Route("GetHastaBilgisi")]
        public IHttpActionResult GetHastaBilgisi(String TC)
        {
            Hasta model = new Hasta();          //model = ent.Hasta.ToList();
            GelenTC = TC;
            model = ent.Hasta.Find(int.Parse(TC));


            return GetSomeData(model);
        }

        [HttpPost]   //Veri gönderir
        [Route("PostHastaBilgisiGuncelle")]
        public void PostHastaBilgisiGuncelle([FromBody] Hasta model)
        {
            Hasta d = new Hasta();

            try
            {
                d = ent.Hasta.Find(model.HastaTC);

                d.HastaTC = model.HastaTC;
                d.HastaAd = model.HastaAd;
                d.HastaSoyad = model.HastaSoyad;
                d.HastaDogumTarihi = model.HastaDogumTarihi;
                d.HastaYas = model.HastaYas;
                d.HastaKanGrubu = model.HastaKanGrubu;
                d.HastaTel = model.HastaTel;
                d.HastaCinsiyet = d.HastaCinsiyet;

                System.Diagnostics.Debug.WriteLine(model.HastaTC);
                System.Diagnostics.Debug.WriteLine(model.HastaAd);

                // Değişiklikleri kaydetme
                ent.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                    }
                }
            }
        }


        [HttpGet]   //Veri çeker
        [Route("GetSomeData")]
        public IHttpActionResult GetSomeData(Hasta hasta)
        {
            var hastaInfoList = new Hasta
            {
                HastaTC = hasta.HastaTC,
                HastaAd = hasta.HastaAd,
                HastaSoyad = hasta.HastaSoyad,
                HastaCinsiyet = hasta.HastaCinsiyet,
                HastaDogumTarihi = hasta.HastaDogumTarihi,
                HastaYas = hasta.HastaYas,
                HastaTel = hasta.HastaTel,
                HastaKanGrubu = hasta.HastaKanGrubu
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,  // Güzel bir görünüm için
                NullValueHandling = NullValueHandling.Ignore,  // Null değerleri göz ardı et
                                                               // Başka yapılandırma seçenekleri
            };

            // JSON içeriğini içeren bir HttpResponseMessage döndürüyoruz.
            // Bu, Web API'nin istemcilere JSON formatında veri göndermesini sağlar.
            return Json(hastaInfoList, jsonSettings);
        }
    }
}
