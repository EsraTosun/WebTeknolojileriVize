using SaglikWebUygulamasi1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaglikWebUygulamasi1.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default

        SaglikDBEntities ent = new SaglikDBEntities();
        static String TC;
        int password;


        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string inputTC, string inputPassword)
        {
            TC = inputTC;
            // Kullanıcı adı ve şifre kontrolü yapılacak
            // Eğer giriş başarılıysa, kullanıcıyı ana sayfaya yönlendirilebilir
            // Eğer giriş başarısızsa, hata mesajı eklenerek tekrar giriş sayfasına yönlendirilebilir
            // Kullanıcı giriş kontrolü burada yapılır.
            if (KullaniciGirisKontrol(inputTC, inputPassword))
            {
                TC = inputTC;
                password = int.Parse(inputPassword);
                // Giriş başarılı ise, kullanıcıyı Index action'ına yönlendir.
                return RedirectToAction("Index", "Home");
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

        public ActionResult Hastalar()
        {
            List<Hasta> model = new List<Hasta>();

            model = ent.Hasta.ToList();
            return View(model);
        }

        public ActionResult Randevular()
        {
            List<Randevu> model = new List<Randevu>();
        
            model = ent.Randevu.Where(a => a.HastaTC.ToString() == TC).ToList();
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

        public ActionResult HastaBilgiGuncelle()
        {
            DuzenleClass model = new DuzenleClass();

            model.HastaList = ent.Hasta.ToList();
            model.loginList = ent.Login.ToList();

            model.duzenlenecekHastaBilgisi = ent.Hasta.Find(int.Parse(TC));

            return View(model);
        }

        [HttpPost]
        public ActionResult KayitDuzenle(String hastaKanGrubu,DateTime hastaDogumTarihi,int hastaYas,int hastaPassword)
        {
            Hasta d = new Hasta();

            d = ent.Hasta.Find(int.Parse(TC));

            d.HastaKanGrubu = hastaKanGrubu;
            d.HastaDogumTarihi = hastaDogumTarihi;
            d.HastaYas = hastaYas;
            d.Login.HastaPassword = hastaPassword;

            ent.SaveChanges();

            return RedirectToAction("OgrenciDers");
        }

    }
}