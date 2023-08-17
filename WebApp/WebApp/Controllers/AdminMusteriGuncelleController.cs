using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMusteriGuncelleController : Controller
    {
        public MakineContext context=new MakineContext();
        public IActionResult Index()
        {
            Musteri Musteri = new Musteri();
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
            Musteri = SessionHelper.GetObjectFromJson<Musteri>(HttpContext.Session, "Musteri");
            
            return View(Musteri);
        }
        [HttpPost]
        public IActionResult Kaydet(Musteri Musteri)
        {



            Musteri.Durumu = 1;

            Musteri.GuncellenmeTarihi = DateTime.Now;

            if (context.Musteris.Any(k => k.AdiSoyadi == Musteri.AdiSoyadi && k.Telefon == Musteri.Telefon && k.Id != Musteri.Id))
            {
                ViewBag.ErrorMessage = "Bu Müşteri Zaten Kayıtlı.";
               
                return View("Index", Musteri);
            }

            if (context.Musteris.Any(k => k.Email == Musteri.Email && k.Id != Musteri.Id))
            {
                ViewBag.ErrorMessage = "Bu E-posta Zaten Kullanılıyor.";
                
                return View("Index", Musteri);
            }
            //if (Musteri.Email == null)
            //{
            //    ViewBag.ErrorMessage = "E-posta Boş Bırakılamaz.";
              
            //    return View("Index", Musteri);
            //}
            //if (Musteri.AdiSoyadi == null)
            //{
            //    ViewBag.ErrorMessage = "Müşteri Adı Soyadı Boş Bırakılamaz.";
                
            //    return View("Index", Musteri);
            //}
            //if (Musteri.Telefon == null)
            //{
            //    ViewBag.ErrorMessage = "Telefon Numarası Boş Bırakılamaz.";
                
            //    return View("Index", Musteri);
            //}

            context.Update(Musteri);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminMusteri");
        }
    }
}
