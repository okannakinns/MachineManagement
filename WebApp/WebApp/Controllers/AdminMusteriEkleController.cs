using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMusteriEkleController : Controller
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
            List<Musteri> MusteriListesi = context.Musteris.ToList();

            return View(MusteriListesi);
        }
        [HttpPost]
        public IActionResult Ekle(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                musteri.OlusturulmaTarihi = DateTime.Now;
                musteri.Durumu = 1;
                musteri.GuncellenmeTarihi = DateTime.Now;

                MakineContext context = new MakineContext();
                if (context.Musteris.Any(k => k.AdiSoyadi == musteri.AdiSoyadi && k.Telefon == musteri.Telefon && k.Id!=musteri.Id))
                {
                    ViewBag.ErrorMessage = "Bu Müşteri Zaten Kayıtlı.";
                    List<Musteri> musteriListesi = context.Musteris.ToList();
                    return View("Index", musteriListesi);
                }

                if (context.Musteris.Any(k => k.Email == musteri.Email))
                {
                    ViewBag.ErrorMessage = "Bu E-posta Zaten Kullanılıyor.";
                    List<Musteri> musteriListesi = context.Musteris.ToList();
                    return View("Index", musteriListesi);
                }
                //if (musteri.Email == null)
                //{
                //    ViewBag.ErrorMessage = "E-posta Boş Bırakılamaz.";
                //    List<Musteri> musteriListesi = context.Musteris.ToList();
                //    return View("Index", musteriListesi);
                //}
                //if (musteri.AdiSoyadi == null)
                //{
                //    ViewBag.ErrorMessage = "Müşteri Adı Soyadı Boş Bırakılamaz.";
                //    List<Musteri> musteriListesi = context.Musteris.ToList();
                //    return View("Index", musteriListesi);
                //}
                //if (musteri.Telefon == null)
                //{
                //    ViewBag.ErrorMessage = "Telefon Numarası Boş Bırakılamaz.";
                //    List<Musteri> musteriListesi = context.Musteris.ToList();
                //    return View("Index", musteriListesi);
                //}
                context.Musteris.Add(musteri);
                context.SaveChanges();

                return RedirectToAction("Index","AdminMusteri");
            }

            return View();
        }
    }
}
