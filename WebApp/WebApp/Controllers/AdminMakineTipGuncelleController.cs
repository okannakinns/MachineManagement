using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineTipGuncelleController : Controller
    {
        public MakineContext context = new MakineContext();
        public IActionResult Index()
        {
            MakineTip MakineTip = new MakineTip();
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
            MakineTip = SessionHelper.GetObjectFromJson<MakineTip>(HttpContext.Session, "MakineTip");
            
            return View(MakineTip);
        }

        [HttpPost]
        public IActionResult Kaydet(MakineTip MakineTip)
        {


            
            MakineTip.Durumu = 1;

            MakineTip.GuncellenmeTarihi = DateTime.Now;

            if (context.MakineTips.Any(k => k.Adi == MakineTip.Adi && k.Id != MakineTip.Id))
            {
               
                return View("Index", MakineTip);
            }
            //if (MakineTip.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Tip Adı Boş Bırakılamaz.";
               
            //    return View("Index", MakineTip);
            //}

            context.Update(MakineTip);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminMakineTip");
        }
    }
}
