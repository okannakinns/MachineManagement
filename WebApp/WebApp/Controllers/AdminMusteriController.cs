using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class AdminMusteriController : Controller
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
            List<Musteri> MusteriListesi = context.Musteris.ToList();

            return View(MusteriListesi);
        }



        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();

            var musteri = dbContext.Musteris.Find(id);
            //var rezervasyon = dbContext.OtelRezervasyons.FirstOrDefault(r => r.MusteriId == id);

            if (musteri != null)
            {
                //if (rezervasyon != null)
                //{
                //    ViewBag.ErrorMessage = "Önce Bu Müşteriye Ait Rezervasyon Kaydını Silmelisiniz.";
                //    List<Musteri> musteriListesi = dbContext.Musteris.ToList();
                //    return View("Index", musteriListesi);
                //}

                dbContext.Remove(musteri);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            Musteri Musteri = context.Musteris.Where(k => k.Id == id).FirstOrDefault();
            if (Musteri == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Musteri", Musteri);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminMusteriGuncelle");
        }
    }
}
