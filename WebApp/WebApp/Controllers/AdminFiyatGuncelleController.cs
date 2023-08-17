using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminFiyatGuncelleController : Controller
    {
        public MakineContext context=new MakineContext();
        public IActionResult Index()
        {
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
            Fiyat Fiyat = new Fiyat();
            Fiyat = SessionHelper.GetObjectFromJson<Fiyat>(HttpContext.Session, "Fiyat");
            ViewBag.MakineTip= context.MakineTips.ToList();
            return View(Fiyat);
        }

        [HttpPost]
        public IActionResult Kaydet(Fiyat Fiyat)
        {


          
            Fiyat.Durumu = 1;

            Fiyat.GuncellenmeTarihi = DateTime.Now;

            if (context.Fiyats.Any(f => f.MakineTipId == Fiyat.MakineTipId && f.Id != Fiyat.Id))
            {
                ViewBag.ErrorMessage = "Bu Makineye Ait Fiyat Bilgisi Zaten Kayıtlı.";
                
                ViewBag.MakineTip = context.MakineTips.ToList();
                return View("Index", Fiyat);

            }
            //if (Fiyat.Saatlik == null)
            //{
            //    ViewBag.ErrorMessage = "Saatlik Fiyat Bölümü Boş Bırakılamaz.";
               
            //    ViewBag.MakineTip = context.MakineTips.ToList();
            //    return View("Index", Fiyat);
            //}
            //if (Fiyat.Gunluk == null)
            //{
            //    ViewBag.ErrorMessage = "Günlük Fiyat Boş Bırakılamaz.";
                
            //    ViewBag.MakineTip = context.MakineTips.ToList();
            //    return View("Index", Fiyat);
            //}
            //if (Fiyat.MakineTipId == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Tipi Boş Bırakılamaz.";
                
            //    ViewBag.MakineTip = context.MakineTips.ToList();
            //    return View("Index", Fiyat);
            //}

            context.Update(Fiyat);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminFiyat");
        }

    }
}
