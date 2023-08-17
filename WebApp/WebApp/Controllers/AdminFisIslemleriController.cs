using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminFisIslemleriController : Controller
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
            ViewBag.Makineler= MakinelerListesi;
            ViewBag.Fiyatlar= FiyatListesi;
            ViewBag.Isler=context.Jobs.ToList();

            return View(FisIslemleriListesi);
            
        }
        [HttpPost]
        public IActionResult Sil(int id)
        {
            
            MakineContext dbContext = new MakineContext();
            var FisIslemleri = dbContext.FisIslemleris.Find(id);

            Is job=new Is();
            job=dbContext.Jobs.FirstOrDefault(j => j.Id==FisIslemleri.IsId);
            if (job != null)
            {
                job.BitisTarihi = null;

            }
            
            if (FisIslemleri != null)
            {
                Makine makine = dbContext.Makines.FirstOrDefault(m => m.Id == FisIslemleri.MakineId);
                TimeSpan SureHesap = (TimeSpan)(FisIslemleri.BitisTarihi - FisIslemleri.BaslangicTarihi);
                if (makine != null)
                {
                    makine.Km -= FisIslemleri.SonKm;
                    makine.ToplamCalismaSaati -= Convert.ToInt32(SureHesap.TotalHours);
                }
                dbContext.Remove(FisIslemleri);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            FisIslemleri FisIslemleri = context.FisIslemleris.Where(k => k.Id == id).FirstOrDefault();
            if (FisIslemleri == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "FisIslemleri", FisIslemleri);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminFisIslemleriGuncelle");
        }
    }
}
