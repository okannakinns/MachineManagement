using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApp.Models;
using System.Dynamic;
namespace WebApp.Controllers
{
    public class AdminIsRaporController : Controller
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
            List<Is> IsListesi = context.Jobs.ToList();
            ViewBag.Musteriler = context.Musteris.ToList();
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
           
            

            foreach (var Is in IsListesi)
            {
                DateTime? tarihVeri = Is.BaslangicTarihi;
                TimeSpan KalanZamanVeri = (TimeSpan)(tarihVeri.Value.Date - DateTime.Now.Date);
                var ayBazliIsVerileri = IsListesi
     .GroupBy(job => job.BaslangicTarihi.Value.Month)
     .Select(group => new AyBazliIsVerisi { Ay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key), IsSayisi = group.Count() })
     .OrderBy(veri => DateTime.ParseExact(veri.Ay, "MMMM", CultureInfo.CurrentCulture)) // Ayları sırala
     .ToList();


                // Ay bazlı iş verilerini Model'e aktar
                ViewBag.AyBazliIsVerileri = ayBazliIsVerileri;

                //ay adları 
                CultureInfo trCulture = new CultureInfo("tr-TR");
                List<string> turkceAyAdlari = new List<string>();

                for (int i = 1; i <= 12; i++)
                {
                    string ayAdi = trCulture.DateTimeFormat.GetMonthName(i);
                    turkceAyAdlari.Add(ayAdi);
                }
                //ay adlarını modele aktar
                ViewBag.TurkceAyAdlari = turkceAyAdlari;

                if (KalanZamanVeri.Days < 0)
                {
                    if (Is.BitisTarihi == null)
                    {

                        Is.KalanZaman = "Başlandı";
                    }
                    else
                    {
                        Is.KalanZaman = "Yapıldı";
                    }

                }
                else if (KalanZamanVeri.TotalHours == 0)
                {
                    Is.KalanZaman = "Bugün";
                }
                else if (KalanZamanVeri.TotalDays >= 1)
                {

                    Is.KalanZaman = KalanZamanVeri.Days.ToString() + " Gün";
                }
            }
            ViewBag.ClosestJob = null;
            ViewBag.Is = IsListesi;
            context.SaveChanges();
            return View(IsListesi);
        }

        public IActionResult MusteriInfo(int MusteriId)
        {
            MakineContext context = new MakineContext();

            // Müşteri Id'sine göre işler tablosundaki verileri filtrele ve OlusturulmaTarihi'ne göre sırala
            var filteredJobs = context.Jobs.Where(j => j.MusteriId == MusteriId)
                                .OrderByDescending(j => j.BaslangicTarihi)
                                .ToList();
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

           
                Musteri musteri = new Musteri();
            musteri = context.Musteris.FirstOrDefault(m => m.Id == MusteriId);
            ViewBag.ContactInfo = musteri;
            // İlk elemanı alarak en yakın tarihe sahip işi bul
            var closestJob = filteredJobs.FirstOrDefault();
            
            // closestJob değişkenini ViewBag ile gönder
            ViewBag.ClosestJob = closestJob;
            ViewBag.Musteriler = context.Musteris.ToList();
            List<Is> IsListesi = context.Jobs.ToList();
            foreach (var Is in IsListesi)
            {
                DateTime? tarihVeri = Is.BaslangicTarihi;
                TimeSpan KalanZamanVeri = (TimeSpan)(tarihVeri.Value.Date - DateTime.Now.Date);
                var ayBazliIsVerileri = IsListesi
    .GroupBy(job => job.BaslangicTarihi.Value.Month)
    .Select(group => new AyBazliIsVerisi { Ay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key), IsSayisi = group.Count() })
    .OrderBy(veri => DateTime.ParseExact(veri.Ay, "MMMM", CultureInfo.CurrentCulture)) // Ayları sırala
    .ToList();


                // Ay bazlı iş verilerini Model'e aktar
                ViewBag.AyBazliIsVerileri = ayBazliIsVerileri;

                //ay adları 
                CultureInfo trCulture = new CultureInfo("tr-TR");
                List<string> turkceAyAdlari = new List<string>();

                for (int i = 1; i <= 12; i++)
                {
                    string ayAdi = trCulture.DateTimeFormat.GetMonthName(i);
                    turkceAyAdlari.Add(ayAdi);
                }
                //ay adlarını modele aktar
                ViewBag.TurkceAyAdlari = turkceAyAdlari;
                //GRAFİK BİLGİLER
                if (KalanZamanVeri.Days < 0)
                {
                    Is.KalanZaman = "Yapıldı";
                }
                else if (KalanZamanVeri.TotalHours == 0)
                {
                    Is.KalanZaman = "Bugün";
                }
                else if (KalanZamanVeri.TotalDays >= 1)
                {

                    Is.KalanZaman = KalanZamanVeri.Days.ToString() + " Gün";
                }
            }
            ViewBag.Is = IsListesi;
            // filteredJobs'un Count sayısını  viewBag e kaydet
            ViewBag.FilteredJobsCount= filteredJobs.Count;

            context.SaveChanges();
            // Verileri hem filteredJobs listesi hem de closestJob değişkeniyle birlikte Index View'ine gönder
            return View("Index");
        }
       
    }
}
