using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineTarihRaporController : Controller
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
            List<FisIslemleri> FisIslemleriListesi = context.FisIslemleris.ToList();
            List<Fiyat> FiyatListesi = context.Fiyats.ToList();
            List<Makine> MakinelerListesi = context.Makines.ToList();
            ViewBag.Makineler = MakinelerListesi;
            ViewBag.Fiyatlar = FiyatListesi;
            ViewBag.Isler = context.Jobs.ToList();

            return View(FisIslemleriListesi);
        }
        [HttpPost]
        public IActionResult Filter(DateTime? BaslangicTarihi, DateTime? BitisTarihi)
        {
            if (BaslangicTarihi.HasValue && BitisTarihi.HasValue)
            {
                MakineContext context = new MakineContext();

                // Tarih aralığına göre verileri filtrele
                var filteredFisIslemleriListesi = context.FisIslemleris
                    .Where(s => s.BaslangicTarihi >= BaslangicTarihi && s.BitisTarihi <= BitisTarihi)
                    .ToList();

                // Gerekli işlemleri yap ve geri döndür
                // ...

                return View("Index", filteredFisIslemleriListesi);
            }

            // Başlangıç ve/veya Bitiş tarihi seçilmemişse, tüm verileri göster
            return RedirectToAction("Index");
        }
    }
}
