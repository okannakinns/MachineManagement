using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
namespace WebApp.Controllers
{
    public class AdminKullaniciEkleController : Controller
    {
        public IActionResult Index()
        {
            //İZİN İŞLEMLERİ//
            string kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (kullaniciAdi == null)
            {
                return RedirectToAction("Index", "Error");
            }
            //İZİN İŞLEMLERİ//
            MakineContext context = new MakineContext();
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
        public IActionResult Ekle(Kullanicilar user)
        {
            
                MakineContext context = new MakineContext();

                user.OlusturulmaTarihi = DateTime.Now;
                user.Durumu = 1;

                user.GuncellenmeTarihi = DateTime.Now;

                if (context.Kullanicilars.Any(k => k.Adi == user.Adi && k.Id!=user.Id))
                {
                    ViewBag.ErrorMessage = "Bu Kullanıcı Adı Zaten Kullanılıyor.";
                    List<Kullanicilar> kullaniciListesi = context.Kullanicilars.ToList();
                    return View("Index", kullaniciListesi);
                }
                
               

                context.Kullanicilars.Add(user);
                context.SaveChanges();
            return RedirectToAction("Index", "AdminKullanici");
        }
            
        }
    }

