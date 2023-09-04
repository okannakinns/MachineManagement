using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
namespace WebApp.Controllers
{
    public class AdminFisYazdirController : Controller
    {
        public IActionResult Index(int id)
        {
            MakineContext context = new MakineContext();
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
           
            List<Fiyat> FiyatListesi = context.Fiyats.ToList();
            List<Makine> MakinelerListesi = context.Makines.ToList();
            ViewBag.Makineler = MakinelerListesi;
            ViewBag.Fiyatlar = FiyatListesi;
            ViewBag.Isler = context.Jobs.ToList();
            ViewBag.FisIslemleri=context.FisIslemleris.Where(i=>i.Id==id);

            var FaturaBilgileri = context.FisIslemleris.FirstOrDefault(i => i.Id == id); // burada ISıd si 9 muş ıs ıdyi burda aldık
            var FaturaUcret = FaturaBilgileri.Ucret;
            
            var isInfo=context.Jobs.FirstOrDefault(context=>context.Id==FaturaBilgileri.IsId);

            var musteriInfo = context.Musteris.FirstOrDefault(m => m.Id == isInfo.MusteriId);
           
            var FaturaMusteriAdi =musteriInfo.AdiSoyadi;
            var FaturaInfo = new List<object> { FaturaMusteriAdi, FaturaUcret };
        

            return View(FaturaInfo);

        }
    }
}
