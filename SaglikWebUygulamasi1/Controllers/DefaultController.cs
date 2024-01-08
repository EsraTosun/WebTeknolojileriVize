﻿using Newtonsoft.Json;
using SaglikWebUygulamasi1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaglikWebUygulamasi1.Controllers
{
    public class LoginCredentials
    {
        public string HastaTC { get; set; }
        public string HastaPassword { get; set; }
        Hasta Hasta;
    }

    public class DefaultController : Controller
    {
        // GET: Default

        SaglikDBEntities ent = new SaglikDBEntities();
        static String TC;
        Hasta hasta = new Hasta();  
        int password;


        [HttpGet]
        public ActionResult Login()
        {
            if(TC != null)
            {
                TC = null;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string inputTC, string inputPassword)
        {
            TC = inputTC;

            // Kullanıcı giriş kontrolü burada yapılır.
            //bool result = KullaniciGirisKontrol(inputTC, inputPassword);
            bool result = await KullaniciGirisKontrol1(inputTC, inputPassword);


            if (result)
            {
                TC = inputTC;
                hasta = ent.Hasta.Find(int.Parse(TC));
                password = int.Parse(inputPassword);
                // Giriş başarılı ise, kullanıcıyı Index action'ına yönlendir.
                return RedirectToAction("HastaBilgi", "Default");
            }
            else
            {
                // Giriş başarısız ise, kullanıcıyı Login sayfasında tut.
                return View();
            }
        }

        private bool KullaniciGirisKontrol(string username, string password)
        {
            List<Login> model = new List<Login>();
            model = ent.Login.ToList();

            foreach (var item in model)
            {
                if (@item.HastaTC.ToString() == username && @item.HastaPassword.ToString() == password)
                {
                    return true;
                }
            }

            // Kullanıcı giriş kontrolü gerçekleştirilir.
            // Bu metot, kullanıcı girişi başarılı ise true, aksi halde false dönmelidir.
            // Giriş kontrolü işlemleri burada gerçekleştirilir.
            return false; // Örnek olarak her zaman başarılı dönüş yapalım.
        }

        private async Task<bool> KullaniciGirisKontrol1(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:44384/api/WebApi/Loginapi/GetKayitliKullanicilar");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Content: " + content);

                    List<Login> modelList = new List<Login>();

                    try
                    {
                        // Doğrudan bir dizi olarak deserializasyon yap
                        modelList = JsonConvert.DeserializeObject<List<Login>>(content);

                        foreach (var item in modelList)
                        {
                            if (item.HastaTC.ToString() == username && item.HastaPassword.ToString() == password)
                            {
                                return true;
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Hata: " + ex.ToString());
                        System.Diagnostics.Debug.WriteLine("Hata: " + ex.Message);
                        // Hata ayrıntılarını yazdır
                    }
                }
            }

            return false;
        }

        [HttpGet]
        public ActionResult SifremiUnuttum()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SifremiUnuttum(string inputTC,DateTime inputDogumTarihi, int inputPassword)
        {
            TC = inputTC;

            // Kullanıcı giriş kontrolü burada yapılır.
            bool result = KullaniciSifreUnuttum(inputTC,inputDogumTarihi, inputPassword);
            //bool result = await KullaniciGirisKontrol1(inputTC, inputPassword);


            if (result)
            {
                TC = inputTC;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Giriş başarısız ise, kullanıcıyı Login sayfasında tut.
                return View();
            }
        }

        private bool KullaniciSifreUnuttum(string username,DateTime dogumTarihi, int password)
        {
            List<Login> model = new List<Login>();
            model = ent.Login.ToList();

            foreach (var item in model)
            {
                if (@item.HastaTC.ToString() == username && @item.Hasta.HastaDogumTarihi.Date == dogumTarihi.Date)
                {
                    Login d = new Login();

                    d = ent.Login.Find(int.Parse(TC));

                    d.HastaTC = int.Parse(TC);
                    d.HastaPassword = password;

                    ent.SaveChanges();
                    return true;
                }
            }
           
            return false; // Örnek olarak her zaman başarılı dönüş yapalım.
        }

        public async Task<ActionResult> Hastaneler()
        {
            List<Hastane> model = new List<Hastane>();

            //model = await HastaneGet();
            model = ent.Hastane.ToList();
            return View(model);
        }

        public async Task<List<Hastane>> HastaneGet()
        {
            List<Hastane> modelList = new List<Hastane>();

            using (var httpClient = new HttpClient())
            {
                //List<Hastane> model = new List<Hastane>();
                var response = await httpClient.GetAsync("https://localhost:44384/api/Hastaneapi/GetHastane");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Content: " + content);

                    //List<Hastane> modelList = new List<Hastane>();

                    // Doğrudan bir dizi olarak deserializasyon yap
                    modelList = JsonConvert.DeserializeObject<List<Hastane>>(content);
                }
            }

            //model = ent.Hastane.ToList();
            return modelList;
        }

        public ActionResult Randevular()
        {
            List<Randevu> model = new List<Randevu>();
        
            model = ent.Randevu.Where(a => a.HastaTC.ToString() == TC).ToList();
            return View(model);
        }

        public ActionResult TahlilBilgisi()
        {
            List<TahlilBilgisi> model = new List<TahlilBilgisi>();

            model = ent.TahlilBilgisi.Where(a => a.HastaTC.ToString() == TC).ToList();
            return View(model);
        }

        public ActionResult HastalikBilgisi()
        {
            List<HastalikBilgisi> model = new List<HastalikBilgisi>();

            model = ent.HastalikBilgisi.Where(a => a.HastaTC.ToString() == TC).ToList();
            return View(model);
        }

        public ActionResult YeniKayit()
        {
            YeniKayitClass model = new YeniKayitClass();

            model.loginList = ent.Login.ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult YeniKayitEkle(String yeniKullaniciTC, String yeniKullaniciPassword)
        {
            // Kullanıcıyı veritabanında kontrol et
            bool kullaniciVarMi = ent.Login.Any(x => x.HastaTC.ToString() == yeniKullaniciTC);

            if (kullaniciVarMi)
            {
                // Kullanıcı zaten var, uyarı ver
                TempData["Uyari"] = "Bu TC numarasına sahip bir kullanıcı zaten kayıtlı.";
                return RedirectToAction("Login", "Default");
            }

            Login m = new Login();

            m.HastaTC = int.Parse(yeniKullaniciTC);
            m.HastaPassword = int.Parse(yeniKullaniciPassword);
            TC = yeniKullaniciTC;

            ent.Login.Add(m);
            ent.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult HastaBilgi()
        {
            DuzenleClass model = new DuzenleClass();

            model.HastaList = ent.Hasta.ToList();
            model.loginList = ent.Login.ToList();

            //model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));
            model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));
            // DuzenleClass'ı liste içine alın
            List<DuzenleClass> modelList = new List<DuzenleClass> { model };

            return View(model);
        }

        public ActionResult HastaBilgiGuncelle()
        {
            DuzenleClass model = new DuzenleClass();

            model.HastaList = ent.Hasta.ToList();
            model.loginList = ent.Login.ToList();

            //model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));
            model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));
            // DuzenleClass'ı liste içine alın
            List<DuzenleClass> modelList = new List<DuzenleClass> { model };

            return View(model);
        }

        [HttpPost]
        public ActionResult HastaBilgiGuncelle(string ad, string soyad, DateTime dogumTarihi, int yas, string kanGrubu, int tel)
        {
            // Formdan gelen bilgileri model üzerinden alabilirsiniz.
            // Örneğin, formunuzdaki alanlar model içinde HastaKanGrubu, HastaDogumTarihi, HastaYas, HastaPassword gibi özelliklere karşılık geliyorsa:

            Hasta d = new Hasta();

            try
            {
                d = ent.Hasta.Find(int.Parse(TC));

                d.HastaTC = int.Parse(TC);
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
            // Güncellenmiş modeli View'e gönder
            return RedirectToAction("HastaBilgi");
        }

        public ActionResult SifreGuncelleme()
        {
            Login model = new Login();
            model = ent.Login.Find(int.Parse(TC));

            return View(model);
        }

        [HttpPost]
        public ActionResult SifreGuncelleme(int password)
        {
            Login d = new Login();

            d = ent.Login.Find(int.Parse(TC));

            d.HastaTC = int.Parse(TC);
            d.HastaPassword = password;

            ent.SaveChanges();

            return View(d);
        }
    }
}