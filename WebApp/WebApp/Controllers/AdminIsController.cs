using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminIsController : Controller
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

            foreach (var Is in IsListesi)
            {
                DateTime? tarihVeri = Is.BaslangicTarihi;
                TimeSpan KalanZamanVeri = (TimeSpan)(tarihVeri.Value.Date - DateTime.Now.Date);

                    if (KalanZamanVeri.Days < 0)
                {
                    if(Is.BitisTarihi==null) {

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
            }
            List<Musteri> MusteriListesi = context.Musteris.ToList();
           
            ViewBag.Musteriler = MusteriListesi;

            context.SaveChanges();
            return View(IsListesi);

        }
        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();
            var Is = dbContext.Jobs.Find(id);

            FisIslemleri FisIslemleri = new FisIslemleri();
            FisIslemleri=dbContext.FisIslemleris.FirstOrDefault(s => s.IsId == id);

            if (FisIslemleri != null)
            {
                ViewBag.ErrorMessage = "Önce Bu İşe ait Sayaç Verisini Silmelisiniz.";
                List<Is> IsListesi = dbContext.Jobs.ToList();


                List<Musteri> MusteriListesi = dbContext.Musteris.ToList();

                ViewBag.Musteriler = MusteriListesi;


                return RedirectToAction("Index");
            }

            if (Is != null)
            {

                dbContext.Remove(Is);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            Is Is = context.Jobs.Where(k => k.Id == id).FirstOrDefault();
            if (Is == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Is", Is);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminIsGuncelle");
        }
    }
}
