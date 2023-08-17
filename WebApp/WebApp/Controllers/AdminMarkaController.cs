using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMarkaController : Controller
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
            List<Marka> MarkaListesi = context.Markas.ToList();

            return View(MarkaListesi);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();
            var Marka = dbContext.Markas.Find(id);
            var model = dbContext.Models.FirstOrDefault(o => o.MarkaId == id);
            if (Marka != null)
            {
                if (model != null)
                {
                    ViewBag.ErrorMessage = "Önce Bu Markanın Bulunduğu Model Kaydını Değiştirmelisiniz. ";
                    List<Marka> MarkaListesi = dbContext.Markas.ToList();
                    return View("Index", MarkaListesi);
                }
                dbContext.Remove(Marka);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            Marka Marka = context.Markas.Where(k => k.Id == id).FirstOrDefault();
            if (Marka == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Marka", Marka);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminMarkaGuncelle");
        }
    }
}
