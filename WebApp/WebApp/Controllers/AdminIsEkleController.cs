using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminIsEkleController : Controller
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
            List<Is> IsListesi = context.Jobs.ToList();
            
            ViewBag.Musteriler = context.Musteris.ToList();
            

            return View(IsListesi);

        }
        [HttpPost]
        public IActionResult Ekle(Is Is)
        {
            MakineContext context = new MakineContext();
           
            Is.OlusturulmaTarihi = DateTime.Now;
            Is.Durumu = 1;
            DateTime? tarihVeri = Is.BaslangicTarihi;
            TimeSpan KalanZamanVeri = (TimeSpan)(tarihVeri.Value.Date - DateTime.Now.Date);

            if (KalanZamanVeri.Days < 0)
            {
                if (Is.BitisTarihi == null)
                {

                    Is.KalanZaman = "Başlandı";
                }
                else
                {
                    Is.KalanZaman = "Yapıldı";
                }

            }
            else if (KalanZamanVeri.TotalHours == 0)
            {
                Is.KalanZaman = "Bugün";
            }
            else if (KalanZamanVeri.TotalDays >= 1)
            {

                Is.KalanZaman = KalanZamanVeri.Days.ToString() + " Gün";
            }
            Is.GuncellenmeTarihi = DateTime.Now;
            
            if (context.Jobs.Any(k => k.Adi == Is.Adi && k.Id != Is.Id))
            {
                ViewBag.ErrorMessage = "Bu İş Zaten Kayıtlı.";
                List<Is> IsListesi = context.Jobs.ToList();
                
                ViewBag.Musteriler = context.Musteris.ToList();
                
                return View("Index", IsListesi);
            }
            //if (Is.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "İş Adı Boş Bırakılamaz.";
            //    List<Is> IsListesi = context.Jobs.ToList();
               
            //    ViewBag.Musteriler = context.Musteris.ToList();
               
            //    return View("Index", IsListesi);
            //}
            context.Jobs.Add(Is);
            context.SaveChanges();

            return RedirectToAction("Index", "AdminIs");
        }
    }
}
