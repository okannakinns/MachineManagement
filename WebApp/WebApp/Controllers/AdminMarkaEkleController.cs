using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMarkaEkleController : Controller
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
            List<Marka> MarkaListesi = context.Markas.ToList();

            return View(MarkaListesi);
        }

        [HttpPost]
        public IActionResult Ekle(Marka Marka)
        {

            Marka.OlusturulmaTarihi = DateTime.Now;
            Marka.Durumu = 1;
            Marka.GuncellenmeTarihi = DateTime.Now;
            MakineContext context = new MakineContext();
            if (context.Markas.Any(k => k.Adi == Marka.Adi && k.Id != Marka.Id))
            {
                ViewBag.ErrorMessage = "Bu Marka Zaten Kayıtlı.";
                List<Marka> MarkaListesi = context.Markas.ToList();
                return View("Index", MarkaListesi);
            }
            //if (Marka.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Marka Adı Boş Bırakılamaz.";
            //    List<Marka> MarkaListesi = context.Markas.ToList();
            //    return View("Index", MarkaListesi);
            //}
            context.Markas.Add(Marka);
            context.SaveChanges();

            return RedirectToAction("Index", "AdminMarka");
        }
    }
}

