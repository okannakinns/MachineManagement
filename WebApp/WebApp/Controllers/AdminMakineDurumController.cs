using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineDurumController : Controller
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
            List<MakineDurum> MakineDurumListesi = context.MakineDurums.ToList();

            return View(MakineDurumListesi);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();
            var MakineDurum = dbContext.MakineDurums.Find(id);
            var makinelar = dbContext.Makines.FirstOrDefault(o => o.MakineDurumId == id);
            if (MakineDurum != null)
            {
                if (makinelar != null)
                {
                    ViewBag.ErrorMessage = "Önce Bu Durumun Bulunduğu Makine Kaydını Değiştirmelisiniz. ";
                    List<MakineDurum> MakineDurumListesi = dbContext.MakineDurums.ToList();
                    return View("Index", MakineDurumListesi);
                }
                dbContext.Remove(MakineDurum);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            MakineDurum MakineDurum = context.MakineDurums.Where(k => k.Id == id).FirstOrDefault();
            if (MakineDurum == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "MakineDurum", MakineDurum);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminMakineDurumGuncelle");
        }
    }
}
