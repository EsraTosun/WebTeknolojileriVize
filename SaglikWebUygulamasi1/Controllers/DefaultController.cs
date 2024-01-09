using Newtonsoft.Json;
using SaglikWebUygulamasi1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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
            bool result = await KullaniciGirisKontrol(inputTC, inputPassword);


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

        private async Task<bool> KullaniciGirisKontrol(string username, string password)
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
            bool result = await KullaniciSifreUnuttum(inputTC,inputDogumTarihi, inputPassword);
            //bool result = await KullaniciGirisKontrol1(inputTC, inputPassword);


            if (result)
            {
                TC = inputTC;

                return RedirectToAction("HastaBilgi", "Default");
            }
            else
            {
                // Giriş başarısız ise, kullanıcıyı Login sayfasında tut.
                return View();
            }
        }

        private async Task<bool> KullaniciSifreUnuttum(string username,DateTime dogumTarihi, int password)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:44384/api/WebApi/Loginapi/GetKayitliKullanicilar");
            var content = await response.Content.ReadAsStringAsync();
            List<Login> modelList = new List<Login>();
            // Doğrudan bir dizi olarak deserializasyon yap
            modelList = JsonConvert.DeserializeObject<List<Login>>(content);
            //model = ent.Login.ToList();

            foreach (var item in modelList)
            {
                if (@item.HastaTC.ToString() == username && @item.Hasta.HastaDogumTarihi.Date == dogumTarihi.Date)
                {

                    using (HttpClient client = new HttpClient())
                    {
                        string apiUrl = "https://localhost:44384/api/WebApi/Loginapi/PostKullaniciGuncelle";

                        var data = new { HastaTC = username, HastaPassword = password };
                        var json = JsonConvert.SerializeObject(data);

                        // İsteği oluştur
                        var content2 = new StringContent(json, Encoding.UTF8, "application/json");

                        // İsteği gönder ve cevabı al
                        try
                        {
                            // İsteği gönder ve cevabı al
                            HttpResponseMessage response2 = await client.PostAsync(apiUrl, content2);

                            // Cevabı kontrol et
                            if (response2.IsSuccessStatusCode)
                            {
                                System.Diagnostics.Debug.WriteLine("Veri güncelleme başarılı!");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Hata kodu: {response2.StatusCode}, Hata açıklaması: {response2.ReasonPhrase}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"İstek gönderilirken bir hata oluştu: {ex.Message}");
                        }
                    }
                    return true;
                }
            }
           
            return false; // Örnek olarak her zaman başarılı dönüş yapalım.
        }

        public ActionResult YeniKayit()
        {
            YeniKayitClass model = new YeniKayitClass();

            model.loginList = ent.Login.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> YeniKayitEkle(String yeniKullaniciTC, String yeniKullaniciPassword)
        {
            // Kullanıcıyı veritabanında kontrol et
            bool kullaniciVarMi = ent.Login.Any(x => x.HastaTC.ToString() == yeniKullaniciTC);

            if (kullaniciVarMi)
            {
                // Kullanıcı zaten var, uyarı ver
                TempData["Uyari"] = "Bu TC numarasına sahip bir kullanıcı zaten kayıtlı.";
                return RedirectToAction("Login", "Default");
            }
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:44384/api/WebApi/Loginapi/PostKullaniciEkle";

                var data = new { HastaTC = int.Parse(yeniKullaniciTC), HastaPassword = yeniKullaniciPassword };
                var json = JsonConvert.SerializeObject(data);

                // İsteği oluştur
                var content2 = new StringContent(json, Encoding.UTF8, "application/json");

                // İsteği gönder ve cevabı al
                try
                {
                    // İsteği gönder ve cevabı al
                    HttpResponseMessage response2 = await client.PostAsync(apiUrl, content2);

                    // Cevabı kontrol et
                    if (response2.IsSuccessStatusCode)
                    {
                        TC = yeniKullaniciTC;
                        System.Diagnostics.Debug.WriteLine("Veri ekleme başarılı!");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Hata kodu: {response2.StatusCode}, Hata açıklaması: {response2.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"İstek gönderilirken bir hata oluştu: {ex.Message}");
                }
            }

            return RedirectToAction("HastaBilgi", "Default");
        }

        public ActionResult SifreGuncelleme()
        {
            Login model = new Login();
            model = ent.Login.Find(int.Parse(TC));

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SifreGuncelleme(int password)
        {
            Login d = new Login();

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:44384/api/WebApi/Loginapi/PostKullaniciGuncelle";

                var data = new { HastaTC = int.Parse(TC), HastaPassword = password };
                var json = JsonConvert.SerializeObject(data);

                // İsteği oluştur
                var content2 = new StringContent(json, Encoding.UTF8, "application/json");

                // İsteği gönder ve cevabı al
                try
                {
                    // İsteği gönder ve cevabı al
                    HttpResponseMessage response2 = await client.PostAsync(apiUrl, content2);

                    // Cevabı kontrol et
                    if (response2.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine("Veri güncelleme başarılı!");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Hata kodu: {response2.StatusCode}, Hata açıklaması: {response2.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"İstek gönderilirken bir hata oluştu: {ex.Message}");
                }
            }
            d = ent.Login.Find(int.Parse(TC));

            return View(d);
        }

        public async Task<ActionResult> Hastaneler()
        {
            List<Hastane> model = new List<Hastane>();

            model = await HastaneGet();
            //model = ent.Hastane.ToList();
            return View(model);
        }

        public async Task<List<Hastane>> HastaneGet()
        {
            List<Hastane> modelList = new List<Hastane>();

            using (var httpClient = new HttpClient())
            {
                //List<Hastane> model = new List<Hastane>();
                var response = await httpClient.GetAsync("https://localhost:44384/api/WebApi/Hastaneapi/GetHastane");

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

        public async Task<ActionResult> Randevular()
        {
            List<Randevu> model = new List<Randevu>();
            model = await RandevuGet();
            //model = ent.Randevu.Where(a => a.HastaTC.ToString() == TC).ToList();
            return View(model);
        }

        public async Task<List<Randevu>> RandevuGet()
        {
            List<Randevu> modelList = new List<Randevu>();

            using (var httpClient = new HttpClient())
            {
                string apiUrl = "https://localhost:44384/api/WebApi/Randevuapi/GetRandevu";
                string requestUrl = $"{apiUrl}?TC={TC}";

                //List<Hastane> model = new List<Hastane>();
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Content: " + content);

                    //List<Hastane> modelList = new List<Hastane>();

                    // Doğrudan bir dizi olarak deserializasyon yap
                    modelList = JsonConvert.DeserializeObject<List<Randevu>>(content);
                }
            }

            //model = ent.Hastane.ToList();
            return modelList;
        }

        public async Task<ActionResult> TahlilBilgisi()
        {
            List<TahlilBilgisi> model = new List<TahlilBilgisi>();

            model = await TahlilBilgisiGet();
            return View(model);
        }

        public async Task<List<TahlilBilgisi>> TahlilBilgisiGet()
        {
            List<TahlilBilgisi> modelList = new List<TahlilBilgisi>();

            using (var httpClient = new HttpClient())
            {
                string apiUrl = "https://localhost:44384/api/WebApi/Tahlilbilgisiapi/GetTahlilBilgisi";
                string requestUrl = $"{apiUrl}?TC={TC}";

                //List<Hastane> model = new List<Hastane>();
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Doğrudan bir dizi olarak deserializasyon yap
                    modelList = JsonConvert.DeserializeObject<List<TahlilBilgisi>>(content);
                }
            }

            //model = ent.Hastane.ToList();
            return modelList;
        }

        public async Task<ActionResult> HastalikBilgisi()
        {
            List<HastalikBilgisi> model = new List<HastalikBilgisi>();

            model = await HastalikBilgisiGet();
            return View(model);
        }

        public async Task<List<HastalikBilgisi>> HastalikBilgisiGet()
        {
            List<HastalikBilgisi> modelList = new List<HastalikBilgisi>();

            using (var httpClient = new HttpClient())
            {
                string apiUrl = "https://localhost:44384/api/WebApi/Hastalikbilgileriapi/GetHastalikBilgisi";
                string requestUrl = $"{apiUrl}?TC={TC}";

                //List<Hastane> model = new List<Hastane>();
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Doğrudan bir dizi olarak deserializasyon yap
                    modelList = JsonConvert.DeserializeObject<List<HastalikBilgisi>>(content);
                }
            }

            return modelList;
        }

        public async Task<ActionResult> HastaBilgi()
        {
            DuzenleClass model = new DuzenleClass();

            model.HastaList = ent.Hasta.ToList();
            model.loginList = ent.Login.ToList();

            //model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));
            model.duzenlenecekHastaBilgisi = await HastaBilgiGet();
            // DuzenleClass'ı liste içine alın
            List<DuzenleClass> modelList = new List<DuzenleClass> { model };

            return View(model);
        }

        public async Task<Hasta> HastaBilgiGet()
        {
            Hasta modelList = new Hasta();

            using (var httpClient = new HttpClient())
            {
                string apiUrl = "https://localhost:44384/api/WebApi/Hastabilgileriapi/GetHastaBilgisi";
                string requestUrl = $"{apiUrl}?TC={TC}";

                //List<Hastane> model = new List<Hastane>();
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Doğrudan bir dizi olarak deserializasyon yap
                    modelList = JsonConvert.DeserializeObject<Hasta>(content);
                }
            }

            return modelList;
        }

        [HttpGet]
        public async Task<ActionResult> HastaBilgiGuncelle()
        {
            DuzenleClass model = new DuzenleClass();

            //model.HastaList = ent.Hasta.ToList();
            //model.loginList = ent.Login.ToList();

            //model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));
            model.duzenlenecekHastaBilgisi = await HastaBilgiGet();
            // DuzenleClass'ı liste içine alın
            //List<DuzenleClass> modelList = new List<DuzenleClass> { model };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HastaBilgiGuncelle(DateTime dogumTarihi, int yas, string kanGrubu, int tel)
        {
            // Formdan gelen bilgileri model üzerinden alabilirsiniz.
            // Örneğin, formunuzdaki alanlar model içinde HastaKanGrubu, HastaDogumTarihi, HastaYas, HastaPassword gibi özelliklere karşılık geliyorsa:

            Hasta d = new Hasta();
            d = await HastaBilgiGet();

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:44384/api/WebApi/Hastabilgileriapi/PostHastaBilgisiGuncelle";

                    var data = new
                    {
                        HastaTC = int.Parse(TC), 
                        HastaAd = d.HastaAd,
                        HastaSoyad = d.HastaSoyad,
                        HastaDogumTarihi = dogumTarihi,
                        HastaYas = yas,
                        HastaKanGrubu = kanGrubu,
                        HastaTel = tel,
                        HastaCinsiyet = d.HastaCinsiyet, 
                    }; 
                    var json = JsonConvert.SerializeObject(data);

                    // İsteği oluştur
                    var content2 = new StringContent(json, Encoding.UTF8, "application/json");

                    // İsteği gönder ve cevabı al
                    try
                    {
                        // İsteği gönder ve cevabı al
                        HttpResponseMessage response2 = await client.PostAsync(apiUrl, content2);

                        // Cevabı kontrol et
                        if (response2.IsSuccessStatusCode)
                        {
                            System.Diagnostics.Debug.WriteLine("Veri güncelleme başarılı!");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Hata kodu: {response2.StatusCode}, Hata açıklaması: {response2.ReasonPhrase}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"İstek gönderilirken bir hata oluştu: {ex.Message}");
                    }
                }
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
    }
}