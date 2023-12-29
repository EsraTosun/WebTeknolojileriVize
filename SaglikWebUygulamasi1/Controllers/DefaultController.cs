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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string inputEmail, string inputPassword)
        {
            // Kullanıcı adı ve şifre kontrolü yapılacak
            // Eğer giriş başarılıysa, kullanıcıyı ana sayfaya yönlendirilebilir
            // Eğer giriş başarısızsa, hata mesajı eklenerek tekrar giriş sayfasına yönlendirilebilir
            // Kullanıcı giriş kontrolü burada yapılır.
            if (KullaniciGirisKontrol(inputEmail, inputPassword))
            {
                // Giriş başarılı ise, kullanıcıyı Index action'ına yönlendir.
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Giriş başarısız ise, kullanıcıyı Login sayfasında tut.
                return View();
            }
            //return RedirectToAction("Index", "Home"); // Örnek bir yönlendirme
        }  
        /*public ActionResult Index()
        {
            return View();
        }*/

        private bool KullaniciGirisKontrol(string username, string password)
        {
            // Kullanıcı giriş kontrolü gerçekleştirilir.
            // Bu metot, kullanıcı girişi başarılı ise true, aksi halde false dönmelidir.
            // Giriş kontrolü işlemleri burada gerçekleştirilir.
            return true; // Örnek olarak her zaman başarılı dönüş yapalım.
        }
    }
}