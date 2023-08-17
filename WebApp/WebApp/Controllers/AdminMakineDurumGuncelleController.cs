using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineDurumGuncelleController : Controller
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
            MakineDurum MakineDurum = new MakineDurum();
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
            MakineDurum = SessionHelper.GetObjectFromJson<MakineDurum>(HttpContext.Session, "MakineDurum");
            
            return View(MakineDurum);
        }

        [HttpPost]
        public IActionResult Kaydet(MakineDurum MakineDurum)
        {



            MakineDurum.Durumu = 1;

            MakineDurum.GuncellenmeTarihi = DateTime.Now;

            if (context.MakineDurums.Any(k => k.Adi == MakineDurum.Adi && k.Id != MakineDurum.Id))
            {
                ViewBag.ErrorMessage = "Bu Makine Durumu Zaten Kayıtlı.";
                
                return View("Index",MakineDurum);
            }
            //if (MakineDurum.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Durum Adı Boş Bırakılamaz.";
               
            //    return View("Index", MakineDurum);
            //}

            context.Update(MakineDurum);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminMakineDurum");
        }
    }
}
