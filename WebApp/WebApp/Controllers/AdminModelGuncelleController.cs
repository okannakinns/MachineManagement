using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminModelGuncelleController : Controller
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
            Model Model = new Model();
            Model = SessionHelper.GetObjectFromJson<Model>(HttpContext.Session, "Model");
         
            List<Marka> MarkaListesi = context.Markas.ToList();
            ViewBag.Markalar = MarkaListesi;
            return View(Model);
        }

        [HttpPost]
        public IActionResult Kaydet(Model Model)
        {



            Model.Durumu = 1;

            Model.GuncellenmeTarihi = DateTime.Now;

            if (context.Models.Any(k => k.Adi == Model.Adi && k.Id != Model.Id))
            {
                ViewBag.ErrorMessage = "Bu Makine Modeli Zaten Kayıtlı.";
               
                List<Marka> MarkaListesi = context.Markas.ToList();
                ViewBag.Markalar = MarkaListesi;
                return View("Index", Model);
            }
            //if (Model.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Model Adı Boş Bırakılamaz.";
               
            //    List<Marka> MarkaListesi = context.Markas.ToList();
            //    ViewBag.Markalar = MarkaListesi;
            //    return View("Index", Model);
            //}

            context.Update(Model);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminModel");
        }
    }
}
