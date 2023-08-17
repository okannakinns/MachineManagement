using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace WebApp.Controllers
{
   
    public class AdminKullaniciGuncelleController : Controller
    {
        public MakineContext context = new MakineContext();
        public IActionResult Index()
        {
            Kullanicilar kullanici = new Kullanicilar();
             kullanici = SessionHelper.GetObjectFromJson<Kullanicilar>(HttpContext.Session, "Kullanici");
            //LAYOUT BİLDİRİM//
            var job = context.Jobs.Where(job => job.KalanZaman == "Bugün");
            ViewBag.JobCount = job.Count();
            ViewBag.Jobs = job;
            if (ViewBag.JobCount == 0)
            {
                var threeDay = context.Jobs.Where(job => job.KalanZaman == "1 Gün" || job.KalanZaman == "2 Gün" || job.KalanZaman == "3 Gün");
                ViewBag.threeDay = threeDay;
                ViewBag.threeDayCount = threeDay.Count();
            }


            //LAYOUT BİLDİRİM//
            return View(kullanici);
        }


       
        [HttpPost]
        public IActionResult Kaydet(Kullanicilar user)
        {
           

           
            user.Durumu = 1;

            user.GuncellenmeTarihi = DateTime.Now;

            if (context.Kullanicilars.Any(k => k.Adi == user.Adi && k.Id!=user.Id))
            {
                
                ViewBag.ErrorMessage = "Bu Kullanıcı Adı Zaten Kullanılıyor.";

                return View("Index", user);
            }
            //if (user.Sifre == null)
            //{
               
            //    ViewBag.ErrorMessage = "Şifre Boş Bırakılamaz.";
            //    return View("Index",user);
            //}

            context.Update(user);
            context.SaveChanges();

            return RedirectToAction("Index","AdminKullanici");
            }

           
        }
    }
    
