using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace WebApp.Controllers
{
    public class AdminMakineEkleController : Controller
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
            List<Makine> MakineListesi = context.Makines.Include(x => x.MakineTip).Include(x => x.MakineDurum).Include(x => x.Marka).Include(x => x.Model).ToList();
            List<MakineDurum> MakineDurumListesi = context.MakineDurums.ToList();
            ViewBag.MakineDurumlari = MakineDurumListesi;
            List<Marka> MarkaListesi = context.Markas.ToList();
            ViewBag.Markalar = MarkaListesi;
            List<Model> ModelListesi = context.Models.ToList();
            ViewBag.Modeller = ModelListesi;
            List<MakineTip> MakineTipListesi = context.MakineTips.ToList();
            ViewBag.MakineTipleri = MakineTipListesi;

            return View(MakineListesi);
        }
       
        [HttpPost]
        public IActionResult Ekle(Makine Makine)
        {


            MakineContext context = new MakineContext();
            Makine.OlusturulmaTarihi = DateTime.Now;
            Makine.Durumu = 1;
            Makine.GuncellenmeTarihi = DateTime.Now;
            Makine.ToplamCalismaSaati = 0;


            if (context.Makines.Any(o => o.Adi == Makine.Adi && o.Id != Makine.Id))
            {
                ViewBag.ErrorMessage = "Bu Makine Zaten Kayıtlı.";
                List<Makine> MakineListesi = context.Makines.Include(x => x.MakineTip).Include(x => x.MakineDurum).Include(x => x.Marka).Include(x => x.Model).ToList();
                ViewBag.MakineDurumlari = context.MakineDurums.ToList();
                ViewBag.Markalar = context.Markas.ToList();
                ViewBag.Modeller = context.Models.ToList();
                ViewBag.MakineTipleri = context.MakineTips.ToList();
                return View("Index", MakineListesi);
            }
            //if (Makine.Adi == null)
            //{
            //    ViewBag.ErrorMessage = "Makine İsmi Boş Bırakılamaz.";
            //    List<Makine> MakineListesi = context.Makines.Include(x => x.MakineTip).Include(x => x.MakineDurum).Include(x => x.Marka).Include(x => x.Model).ToList();
            //    ViewBag.MakineDurumlari = context.MakineDurums.ToList();
            //    ViewBag.Markalar = context.Markas.ToList();
            //    ViewBag.Modeller = context.Models.ToList();
            //    ViewBag.MakineTipleri = context.MakineTips.ToList();
            //    return View("Index", MakineListesi);
            //}
            //if (Makine.Plaka == null)
            //{
            //    ViewBag.ErrorMessage = "Plaka Boş Bırakılamaz.";
            //    List<Makine> MakineListesi = context.Makines.Include(x => x.MakineTip).Include(x => x.MakineDurum).Include(x => x.Marka).Include(x => x.Model).ToList();
            //    ViewBag.MakineDurumlari = context.MakineDurums.ToList();
            //    ViewBag.Markalar = context.Markas.ToList();
            //    ViewBag.Modeller = context.Models.ToList();
            //    ViewBag.MakineTipleri = context.MakineTips.ToList();
            //    return View("Index", MakineListesi);
            //}

            context.Makines.Add(Makine);

            context.SaveChanges();

            return RedirectToAction("Index","AdminMakine");
        }

    }
}
