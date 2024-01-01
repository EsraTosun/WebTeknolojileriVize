﻿using Newtonsoft.Json;
using SaglikWebUygulamasi1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaglikWebUygulamasi1.Controllers
{
    [RoutePrefix("api/Loginapi")]
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
        public void PostKullaniciEkle(Login model)
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
            var loginInfoList = model.Select(login => new Login
            {
                HastaTC = login.HastaTC,
                HastaPassword = login.HastaPassword,
            }).ToList();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                // Diğer yapılandırma seçenekleri
            };

            var json = JsonConvert.SerializeObject(loginInfoList, Formatting.Indented, jsonSettings);

            return Json(json);
        }
    }
}