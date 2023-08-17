using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineDurumEkleController : Controller
    {
        public IActionResult Index()
        {
            //İZİN İŞLEMLERİ//
            string kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (kullaniciAdi == null)
            {
                return RedirectToAction("Index", "Error");
            }
            //İZİN İŞLEMLERİ//
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
        public IActionResult Ekle(MakineDurum MakineDurum)
        {

            MakineDurum.OlusturulmaTarihi = DateTime.Now;
            MakineDurum.Durumu = 1;
            MakineDurum.GuncellenmeTarihi = DateTime.Now;
            MakineContext context = new MakineContext();
            if (context.MakineDurums.Any(k => k.Adi == MakineDurum.Adi && k.Id != MakineDurum.Id))
            {
                ViewBag.ErrorMessage = "Bu Makine Durumu Zaten Kayıtlı.";
                List<MakineDurum> MakineDurumListesi = context.MakineDurums.ToList();
                return View("Index", MakineDurumListesi);
            }
            //if (MakineDurum.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Durum Adı Boş Bırakılamaz.";
            //    List<MakineDurum> MakineDurumListesi = context.MakineDurums.ToList();
            //    return View("Index", MakineDurumListesi);
            //}
            context.MakineDurums.Add(MakineDurum);
            context.SaveChanges();

            return RedirectToAction("Index", "AdminMakineDurum");
        }
    }
}

