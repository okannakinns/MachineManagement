using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminIsGuncelleController : Controller
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
            Is Is = new Is();
            Is = SessionHelper.GetObjectFromJson<Is>(HttpContext.Session, "Is");
            
            
            ViewBag.Musteriler = context.Musteris.ToList();
      
            return View(Is);
        }

        [HttpPost]
        public IActionResult Kaydet(Is Is)
        {
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
            FisIslemleri FisIslemleri = new FisIslemleri();
            FisIslemleri = context.FisIslemleris.FirstOrDefault(m => m.IsId == Is.Id);
            if (FisIslemleri != null)
            {
                FisIslemleri.BaslangicTarihi = Is.BaslangicTarihi;
            }

            Is.Durumu = 1;

            Is.GuncellenmeTarihi = DateTime.Now;

            if (context.Jobs.Any(k => k.Adi == Is.Adi && k.Id != Is.Id))
            {
                ViewBag.ErrorMessage = "Bu İş Zaten Kayıtlı.";
                
                ViewBag.Musteriler = context.Musteris.ToList();
                
                return View("Index", Is);
            }
            //if (Is.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "İş Adı Boş Bırakılamaz.";
               
            //    ViewBag.Musteriler = context.Musteris.ToList();
                
            //    return View("Index", Is);
            //}

            context.Update(Is);
            context.SaveChanges();
            return RedirectToAction("Index", "AdminIs");
        }
    }
}
