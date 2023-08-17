using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminFiyatController : Controller
    {
        public MakineContext context = new MakineContext();
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
            List<Fiyat> FiyatListesi = context.Fiyats.Include(x => x.MakineTip).ToList();
            List<MakineTip> MakineTipListesi = context.MakineTips.ToList();
            ViewBag.MakineTip = MakineTipListesi;
            return View(FiyatListesi);
            
        }

        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            Fiyat Fiyat = context.Fiyats.Where(k => k.Id == id).FirstOrDefault();
            if (Fiyat == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Fiyat", Fiyat);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminFiyatGuncelle");
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            var fiyat = context.Fiyats.Find(id);
            //var rezerve = dbContext.OtelRezervasyons.FirstOrDefault(x => x.MakineTipId == fiyat.MakineTipId);
            if (fiyat != null)
            {
                //if (rezerve != null)
                //{
                //    ViewBag.ErrorMessage = "Önce Bu Fiyata Ait Makinenin Rezervasyonunu Silmelisiniz.";
                //    List<Fiyat> FiyatListesi = dbContext.Fiyats.Include(x => x.MakineTip).ToList();
                //    ViewBag.MakineTip = dbcontext.MakineTips.ToList();
                //    return View("Index", FiyatListesi);
                //}
                context.Remove(fiyat);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
