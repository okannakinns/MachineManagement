using Microsoft.AspNetCore.Mvc;
using WebApp.Models;


namespace WebApp.Controllers
{
    public class AdminFisIslemleriEkleController : Controller
    {
        public IActionResult Index()
        {
            MakineContext context = new MakineContext();
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
            List<FisIslemleri> FisIslemleriListesi = context.FisIslemleris.ToList();
            List<Fiyat> FiyatListesi = context.Fiyats.ToList();
            List<Makine> MakinelerListesi = context.Makines.ToList();
            ViewBag.Makineler = MakinelerListesi;
            ViewBag.Fiyatlar = FiyatListesi;
            ViewBag.Isler = context.Jobs.ToList();

            return View(FisIslemleriListesi);
        }
        [HttpPost]
        public IActionResult Ekle(FisIslemleri FisIslemleri)
        {
            MakineContext context = new MakineContext();
            
            Is job =new Is();
            job = context.Jobs.FirstOrDefault(m => m.Id == FisIslemleri.IsId);
            if(job != null) {
            FisIslemleri.BaslangicTarihi=job.BaslangicTarihi;
                job.BitisTarihi=FisIslemleri.BitisTarihi;
            }
            FisIslemleri.OlusturulmaTarihi = DateTime.Now;
            FisIslemleri.Durumu = 1;
            FisIslemleri.GuncellenmeTarihi = DateTime.Now;
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
            

           
            Makine makine = new Makine();
            makine = context.Makines.FirstOrDefault(m=>m.Id==FisIslemleri.MakineId);
            
            if (makine != null)
            {
                if (FisIslemleri.SonKm <= makine.Km)
                {
                    ViewBag.ErrorMessage = "Son Kilometre, makinenin kilometresinden az ya da aynı olamaz.";
                    List<FisIslemleri> FisIslemleriListesi = context.FisIslemleris.ToList();
                    List<Fiyat> FiyatListesi = context.Fiyats.ToList();
                    List<Makine> MakinelerListesi = context.Makines.ToList();
                    ViewBag.Makineler = MakinelerListesi;
                    ViewBag.Fiyatlar = FiyatListesi;
                    ViewBag.Isler = context.Jobs.ToList();

                    return View("Index",FisIslemleriListesi);
                }
                makine.ToplamCalismaSaati+= (int)sureHesap.TotalHours;
                FisIslemleri.SonKm = FisIslemleri.SonKm - makine.Km;
                makine.Km += FisIslemleri.SonKm;
               
            }
            Fiyat fiyat = new Fiyat();
            fiyat= context.Fiyats.FirstOrDefault(m=>m.Id==FisIslemleri.FiyatId);
            if(fiyat != null)
            {
                FisIslemleri.Ucret = fiyat.Saatlik * resultHours;
                FisIslemleri.Ucret += fiyat.Gunluk * resultDays;
            }
           
            context.FisIslemleris.Add(FisIslemleri);
            context.SaveChanges();

            return RedirectToAction("Index", "AdminFisIslemleri");
        }
    }
}
