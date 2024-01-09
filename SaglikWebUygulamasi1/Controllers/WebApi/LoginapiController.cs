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
    [RoutePrefix("api/WebApi/Loginapi")]
    public class LoginapiController : ApiController
    {
        SaglikDBEntities ent = new SaglikDBEntities();

        [HttpGet]   //Veri çeker
        [Route("GetKayitliKullanicilar")]
        public IHttpActionResult GetKayitliKullanicilar() 
        {
            List<Login> model = new List<Login>();
            model = ent.Login.ToList();

            return GetSomeData(model);
        }

        [HttpPost]   //Veri gönderir
        [Route("PostKullaniciGuncelle")]
        public void PostKullaniciGuncelle([FromBody] Login model)
        {
            Login d = new Login();
            d = ent.Login.Find(model.HastaTC);
            d.HastaTC = model.HastaTC;
            d.HastaPassword = model.HastaPassword;
            System.Diagnostics.Debug.WriteLine(model.HastaTC);
            System.Diagnostics.Debug.WriteLine(model.HastaPassword);


            ent.SaveChanges();
        }

        [HttpPost]   //Veri gönderir
        [Route("PostKullaniciEkle")]
        public void PostKullaniciEkle([FromBody] Login model)
        {
            if(ModelState.IsValid) 
            {
                ent.Login.Add(model);
                ent.SaveChanges();
            }
        }
        

        [HttpGet]   //Veri çeker
        [Route("GetSomeData")]
        public IHttpActionResult GetSomeData(List<Login> model)
        {
            // Gelen Login listesini kullanarak yeni bir liste oluşturuyoruz.
            // Bu yeni liste, her bir Login nesnesinin sadece belirli özelliklerini içeriyor.
            var loginInfoList = model.Select(login => new Login
            {
                HastaTC = login.HastaTC,
                HastaPassword = login.HastaPassword,
                Hasta = login.Hasta,
            }).ToList();

            // JSON serileştirme ayarlarını yapılandırıyoruz.
            // Bu ayarlar, dönen JSON'un görüntüsünü düzenlemek ve belirli durumları ele almak için kullanılır.
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,  // Döngüsel referansları ele alır
                Formatting = Formatting.Indented,  // JSON'un okunabilir olması için girinti ekler
                NullValueHandling = NullValueHandling.Ignore,  // Null değerleri göz ardı et
                                                               // Diğer yapılandırma seçenekleri
            };

            // JSON serileştirmeyi gerçekleştiriyoruz.
            // loginInfoList nesnesini JSON formatına çeviriyoruz.
            var json = JsonConvert.SerializeObject(loginInfoList, jsonSettings);

            // JSON içeriğini içeren bir HttpResponseMessage döndürüyoruz.
            // Bu, Web API'nin istemcilere JSON formatında veri göndermesini sağlar.
            return Json(loginInfoList, jsonSettings);
        }


    }
}
