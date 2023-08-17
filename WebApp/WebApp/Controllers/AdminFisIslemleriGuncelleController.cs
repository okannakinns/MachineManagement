using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminFisIslemleriGuncelleController : Controller
    {
        public MakineContext context = new MakineContext();
        public IActionResult Index()
        {
            FisIslemleri FisIslemleri = new FisIslemleri();
            //İZİN İŞLEMLERİ//
            string kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (kullaniciAdi == null)
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
            FisIslemleri = SessionHelper.GetObjectFromJson<FisIslemleri>(HttpContext.Session, "FisIslemleri");
            ViewBag.Fiyatlar = context.Fiyats.ToList();
            ViewBag.Makineler = context.Makines.ToList();
            ViewBag.Isler = context.Jobs.ToList();




            Makine makine = context.Makines.FirstOrDefault(m => m.Id == FisIslemleri.MakineId);
            if (makine != null)
            {
                int kmVeri = makine.Km;

                // Eski çalışma süresi düşürme
                TimeSpan eskiSureHesap = (TimeSpan)(FisIslemleri.BitisTarihi - FisIslemleri.BaslangicTarihi);
                makine.ToplamCalismaSaati -= (int)eskiSureHesap.TotalHours;
                makine.Km -= FisIslemleri.SonKm;
                FisIslemleri.SonKm = kmVeri;
                context.Update(makine);
                context.SaveChanges();

            }
            return View(FisIslemleri);
        }

        [HttpPost]
        public IActionResult Kaydet(FisIslemleri FisIslemleri)
        {
            MakineContext context = new MakineContext();
            Is job = new Is();
            job = context.Jobs.FirstOrDefault(m => m.Id == FisIslemleri.IsId);
            if (job != null)
            {
                FisIslemleri.BaslangicTarihi = job.BaslangicTarihi;
                job.BitisTarihi = FisIslemleri.BitisTarihi;
            }
            if (FisIslemleri.MakineId == null || FisIslemleri.FiyatId == null || FisIslemleri.IsId == null || FisIslemleri.BaslangicTarihi == null || FisIslemleri.BitisTarihi == null || FisIslemleri.SonKm == null)
            {
                ViewBag.ErrorMessage = "Boş Alan Bırakmayınız.";
                ViewBag.Fiyatlar = context.Fiyats.ToList();
                ViewBag.Makineler = context.Makines.ToList();
                ViewBag.Isler = context.Jobs.ToList();

                return View("Index", FisIslemleri);
            }
            Makine makine = context.Makines.FirstOrDefault(m => m.Id == FisIslemleri.MakineId);
            if (makine != null)
            {
                if (FisIslemleri.SonKm <= makine.Km)
                {
                    ViewBag.ErrorMessage = "Son Kilometre, makinenin kilometresinden az ya da aynı olamaz."; 
                    ViewBag.Fiyatlar = context.Fiyats.ToList();
                    ViewBag.Makineler = context.Makines.ToList();
                    ViewBag.Isler = context.Jobs.ToList();

                    return View("Index", FisIslemleri);
                }
            }
            FisIslemleri.Durumu = 1;
            FisIslemleri.GuncellenmeTarihi = DateTime.Now;
            
            
           

            // CalismaSuresi'ni güncelle
            TimeSpan sureHesap = (TimeSpan)(FisIslemleri.BitisTarihi - FisIslemleri.BaslangicTarihi);
            int resultHours = sureHesap.Hours;
            int resultDays = sureHesap.Days;
            if (resultDays == 0)
            {
                FisIslemleri.CalismaSuresi = resultHours + " saat";
            }
            else
            {
                FisIslemleri.CalismaSuresi = resultDays + " gün " + resultHours + " saat";
            }


           
            

            // Ucret'i güncelle
            Fiyat fiyat = context.Fiyats.FirstOrDefault(m => m.Id == FisIslemleri.FiyatId);
            if (fiyat != null)
            {
                FisIslemleri.Ucret = fiyat.Saatlik * resultHours;
                FisIslemleri.Ucret += fiyat.Gunluk * resultDays;
            }
           
            if(makine != null)
            {
                makine.ToplamCalismaSaati += (int)sureHesap.TotalHours;
                FisIslemleri.SonKm = FisIslemleri.SonKm - makine.Km;
                makine.Km += FisIslemleri.SonKm;
                
            }
           

            context.Update(FisIslemleri);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminFisIslemleri");
        }
    }
}
