
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace WebApp.Controllers
{
    public class AdminKullaniciController : Controller
    {
        public MakineContext context = new MakineContext();
        public IActionResult Index()
        {
            //İZİN İŞLEMLERİ//
            string kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if(kullaniciAdi == null)
            {
                return RedirectToAction("Index", "Error");
            }
            //İZİN İŞLEMLERİ//
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
            List<Kullanicilar> KullanicilarListesi = context.Kullanicilars.ToList();

            return View(KullanicilarListesi);
        }

        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            Kullanicilar kullanici = context.Kullanicilars.Where(k => k.Id == id).FirstOrDefault();
            if (kullanici == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Kullanici", kullanici);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminKullaniciGuncelle");
        }





        [HttpPost]
        public IActionResult Sil(int id)
        {
            string kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

            var user = context.Kullanicilars.Find(id);
          
            if (user != null)
            {
                context.Remove(user);
                context.SaveChanges();
                
            }
            if (user.Adi == kullaniciAdi)
            {
                
                return RedirectToAction("Index", "AdminGiris");
            }
            return RedirectToAction("Index", "AdminKullanici");
        }
        }
        }






