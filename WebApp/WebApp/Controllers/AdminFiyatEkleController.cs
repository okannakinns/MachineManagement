using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminFiyatEkleController : Controller
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
            List<Fiyat> FiyatListesi = context.Fiyats.Include(x => x.MakineTip).ToList();
            List<MakineTip> MakineTipListesi = context.MakineTips.ToList();
            ViewBag.MakineTip = MakineTipListesi;
            return View(FiyatListesi);
        }
        [HttpPost]
        public IActionResult Ekle(Fiyat Fiyat)
        {

            Fiyat.OlusturulmaTarihi = DateTime.Now;
            Fiyat.Durumu = 1;
            Fiyat.GuncellenmeTarihi = DateTime.Now;
            MakineContext context = new MakineContext();
            if (context.Fiyats.Any(f => f.MakineTipId == Fiyat.MakineTipId && f.Id != Fiyat.Id))
            {
                ViewBag.ErrorMessage = "Bu Makineye Ait Fiyat Bilgisi Zaten Kayıtlı.";
                List<Fiyat> FiyatListesi = context.Fiyats.Include(x => x.MakineTip).ToList();
                ViewBag.MakineTip = context.MakineTips.ToList();
                return View("Index", FiyatListesi);

            }
            //if (Fiyat.Saatlik == null)
            //{
            //    ViewBag.ErrorMessage = "Saatlik Fiyat Bölümü Boş Bırakılamaz.";
            //    List<Fiyat> FiyatListesi = context.Fiyats.Include(x => x.MakineTip).ToList();
            //    ViewBag.MakineTip = context.MakineTips.ToList();
            //    return View("Index", FiyatListesi);
            //}
            //if (Fiyat.Gunluk == null)
            //{
            //    ViewBag.ErrorMessage = "Günlük Fiyat Boş Bırakılamaz.";
            //    List<Fiyat> FiyatListesi = context.Fiyats.Include(x => x.MakineTip).ToList();
            //    ViewBag.MakineTip = context.MakineTips.ToList();
            //    return View("Index", FiyatListesi);
            //}
            //if (Fiyat.MakineTipId == null)
            //{
            //    ViewBag.ErrorMessage = "Makine Tipi Boş Bırakılamaz.";
            //    List<Fiyat> FiyatListesi = context.Fiyats.Include(x => x.MakineTip).ToList();
            //    ViewBag.MakineTip = context.MakineTips.ToList();
            //    return View("Index", FiyatListesi);
            //}

            context.Fiyats.Add(Fiyat);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminFiyat");
        }

    }
}

