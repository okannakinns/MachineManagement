using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminModelEkleController : Controller
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
            List<Model> ModelListesi = context.Models.Include(m => m.Marka).ToList();
            List<Marka> MarkaListesi = context.Markas.ToList();
            ViewBag.Markalar = MarkaListesi;
            return View("Index", ModelListesi);
        }

        [HttpPost]
        public IActionResult Ekle(Model Model)
        {

            Model.OlusturulmaTarihi = DateTime.Now;
            Model.Durumu = 1;
            Model.GuncellenmeTarihi = DateTime.Now;
            MakineContext context = new MakineContext();
            if (context.Models.Any(k => k.Adi == Model.Adi && k.Id != Model.Id))
            {
                ViewBag.ErrorMessage = "Bu Makine Modeli Zaten Kayıtlı.";
                List<Model> ModelListesi = context.Models.Include(m => m.Marka).ToList();
                List<Marka> MarkaListesi = context.Markas.ToList();
                ViewBag.Markalar = MarkaListesi;
                return View("Index", ModelListesi);
            }
            //if (Model.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Model Adı Boş Bırakılamaz.";
            //    List<Model> ModelListesi = context.Models.Include(m => m.Marka).ToList();
            //    List<Marka> MarkaListesi = context.Markas.ToList();
            //    ViewBag.Markalar = MarkaListesi;
            //    return View("Index", ModelListesi);
            //}
            context.Models.Add(Model);
            context.SaveChanges();

            return RedirectToAction("Index", "AdminModel");
        }
    }
}

