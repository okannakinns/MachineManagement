using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


namespace WebApp.Controllers
{
    public class AdminMakineGuncelleController : Controller
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
            Makine Makine = new Makine();
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
            Makine = SessionHelper.GetObjectFromJson<Makine>(HttpContext.Session, "Makine");
            ViewBag.MakineDurumlari = context.MakineDurums.ToList();
            ViewBag.Markalar = context.Markas.ToList();
            ViewBag.Modeller = context.Models.ToList();
            ViewBag.MakineTipleri = context.MakineTips.ToList();



            return View(Makine);
        }

        [HttpPost]
        public IActionResult Kaydet(Makine Makine)
        {



            Makine.Durumu = 1;
           
            Makine.GuncellenmeTarihi = DateTime.Now;
            if (Makine.ToplamCalismaSaati == null)
            {
                Makine.ToplamCalismaSaati = 0;
            }
            if (context.Makines.Any(o => o.Adi == Makine.Adi && o.Id!=Makine.Id))
            {
                ViewBag.ErrorMessage = "Bu Makine Zaten Kayıtlı.";
                ViewBag.MakineDurumlari = context.MakineDurums.ToList();
                ViewBag.Markalar = context.Markas.ToList();
                ViewBag.Modeller = context.Models.ToList();
                ViewBag.MakineTipleri = context.MakineTips.ToList();
                return View("Index", Makine);
            }
            //if (Makine.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine İsmi Boş Bırakılamaz.";
            //    ViewBag.MakineDurumlari = context.MakineDurums.ToList();
            //    ViewBag.Markalar = context.Markas.ToList();
            //    ViewBag.Modeller = context.Models.ToList();
            //    ViewBag.MakineTipleri = context.MakineTips.ToList();
            //    return View("Index", Makine);
            //}
            //if (Makine.Plaka == null)
            //{
            //    ViewBag.ErrorMessage = "Plaka Boş Bırakılamaz.";
            //    ViewBag.MakineDurumlari = context.MakineDurums.ToList();
            //    ViewBag.Markalar = context.Markas.ToList();
            //    ViewBag.Modeller = context.Models.ToList();
            //    ViewBag.MakineTipleri = context.MakineTips.ToList();
            //    return View("Index", Makine);
            //}

            context.Update(Makine);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminMakine");
        }
    }
}
