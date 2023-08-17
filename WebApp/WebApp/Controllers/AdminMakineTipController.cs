using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineTipController : Controller
    {
        public IActionResult Index()
        {
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
            List<MakineTip> makineTipListesi = context.MakineTips.ToList();

            return View(makineTipListesi);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();
            var makinetip = dbContext.MakineTips.Find(id);
            var makinelar = dbContext.Makines.FirstOrDefault(o => o.MakineTipId == id);
            if (makinetip != null)
            {
                if (makinelar != null)
                {
                    ViewBag.ErrorMessage = "Önce Bu Tipin Bulunduğu Makine Kaydını Değiştirmelisiniz. ";
                    List<MakineTip> makinetipListesi = dbContext.MakineTips.ToList();
                    return View("Index", makinetipListesi);
                }
                dbContext.Remove(makinetip);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            MakineTip MakineTip = context.MakineTips.Where(k => k.Id == id).FirstOrDefault();
            if (MakineTip == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "MakineTip", MakineTip);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminMakineTipGuncelle");
        }
    }
}
