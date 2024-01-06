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
            List<Hasta> model = new List<Hasta>();
            //model = ent.Hasta.ToList();
            GelenTC = TC;
            model = ent.Hasta.Where(a => a.HastaTC.ToString() == TC).ToList();


            return GetSomeData(model);
        }

        [HttpPost]   //Veri gönderir
        public void GetHastaBilgisiGuncelle(string ad, string soyad, DateTime dogumTarihi, int yas, string kanGrubu, int tel)
        {
            Hasta d = new Hasta();

            try
            {
                d = ent.Hasta.Find(int.Parse(GelenTC));

                d.HastaTC = int.Parse(GelenTC);
                d.HastaAd = ad;
                d.HastaSoyad = soyad;
                d.HastaDogumTarihi = dogumTarihi;
                d.HastaYas = yas;
                d.HastaKanGrubu = kanGrubu;
                d.HastaTel = tel;
                d.HastaCinsiyet = d.HastaCinsiyet;

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
        public IHttpActionResult GetSomeData(List<Hasta> model)
        {
            var hastaInfoList = model.Select(hasta => new Hasta
            {
                HastaTC = hasta.HastaTC,
                HastaAd = hasta.HastaAd,
                HastaSoyad = hasta.HastaSoyad,
                HastaCinsiyet = hasta.HastaCinsiyet,
                HastaDogumTarihi = hasta.HastaDogumTarihi,
                HastaYas = hasta.HastaYas,
                HastaTel = hasta.HastaTel,
                HastaKanGrubu = hasta.HastaKanGrubu,
            }).ToList();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,  // Güzel bir görünüm için
                NullValueHandling = NullValueHandling.Ignore,  // Null değerleri göz ardı et
                                                               // Başka yapılandırma seçenekleri
            };

            var json = JsonConvert.SerializeObject(hastaInfoList, Formatting.Indented, jsonSettings);

            return Json(json);
        }
    }
}
