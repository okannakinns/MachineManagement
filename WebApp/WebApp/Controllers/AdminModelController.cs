using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminModelController : Controller
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
            List<Model> ModelListesi = context.Models.Include(m => m.Marka).ToList();
            List<Marka> MarkaListesi = context.Markas.ToList();
            ViewBag.Markalar = MarkaListesi;
            return View(ModelListesi);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();
            var Model = dbContext.Models.Find(id);
            var makinelar = dbContext.Makines.FirstOrDefault(o => o.ModelId == id);
            if (Model != null)
            {
                if (makinelar != null)
                {
                    ViewBag.ErrorMessage = "Önce Bu Modelin Bulunduğu Makine Kaydını Değiştirmelisiniz. ";
                    MakineContext context = new MakineContext();
                    List<Model> ModelListesi = context.Models.Include(m => m.Marka).ToList();
                    List<Marka> MarkaListesi = context.Markas.ToList();
                    ViewBag.Markalar = MarkaListesi;
                    return View("Index",ModelListesi);
                }
                dbContext.Remove(Model);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            Model Model = context.Models.Where(k => k.Id == id).FirstOrDefault();
            if (Model == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Model", Model);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminModelGuncelle");
        }
    }
}
