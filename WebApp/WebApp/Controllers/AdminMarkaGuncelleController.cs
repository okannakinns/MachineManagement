using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMarkaGuncelleController : Controller
    {
        public MakineContext context = new MakineContext();
        public IActionResult Index()
        {
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
            Marka Marka = new Marka();
            Marka = SessionHelper.GetObjectFromJson<Marka>(HttpContext.Session, "Marka");
            
            return View(Marka);
        }

        [HttpPost]
        public IActionResult Kaydet(Marka Marka)
        {



            Marka.Durumu = 1;

            Marka.GuncellenmeTarihi = DateTime.Now;

            if (context.Markas.Any(k => k.Adi == Marka.Adi && k.Id != Marka.Id))
            {
                ViewBag.ErrorMessage = "Bu Marka Zaten Kayıtlı.";
               
                return View("Index", Marka);
            }
            //if (Marka.Adi == null)
            //{
            //    ViewBag.ErrorMessage = " Marka Adı Boş Bırakılamaz.";
                
            //    return View("Index", Marka);
            //}

            context.Update(Marka);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminMarka");
        }
    }
}
