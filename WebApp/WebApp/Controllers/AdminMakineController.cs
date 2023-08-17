using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


namespace WebApp.Controllers
{
    public class AdminMakineController : Controller
    {
        public IActionResult Index()
        {
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
        [HttpGet]
        public IActionResult Models(int markaId)
        {
            MakineContext context = new MakineContext();
            List<Model> models = context.Models.Where(x => x.MarkaId == markaId).ToList();

            return Json(models);
        }
        [HttpPost]
        public IActionResult Guncelle(int id)
        {
            MakineContext context = new MakineContext();
            Makine Makine = context.Makines.Where(k => k.Id == id).FirstOrDefault();
            if (Makine == null)
            {
                return RedirectToAction("Index");
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Makine", Makine);


            // Güncelleme sayfasına id değerini göndererek yönlendirme yapın
            return RedirectToAction("Index", "AdminMakineGuncelle");
        }
        [HttpPost]
        public IActionResult Sil(int id)
        {
            MakineContext dbContext = new MakineContext();
            var makine = dbContext.Makines.Find(id);
            //var rezervasyon = dbContext.OtelRezervasyons.FirstOrDefault(r => r.MakineId == id);
            //var fiyat = dbContext.Fiyats.FirstOrDefault(r => r.MakineId == id);
            //var fiyatcarpan = dbContext.FiyatCarpans.FirstOrDefault(r => r.MakineId == id);
            //var MakineKontenjan = dbContext.MakineKontenjans.FirstOrDefault(r => r.MakineId == id);


            //if (Makine != null)
            //{
            //    if (rezervasyon != null)
            //    {
            //        ViewBag.ErrorMessage = "Önce Bu Makineya Ait Rezervasyon Kaydını Silmelisiniz.";
            //        List<Makine> MakineListesi = dbContext.Makines.Include(x => x.YatakTip).Include(x => x.MakineTip).Include(x => x.MakineDurum).ToList();
            //        ViewBag.MakineDurumlari = dbContext.MakineDurums.ToList();
            //        ViewBag.YatakTipleri = dbContext.YatakTips.ToList();
            //        ViewBag.MakineTipleri = dbContext.MakineTips.ToList();
            //        return View("Index", MakineListesi);
            //    }

            //    if (fiyat != null)
            //    {
            //        ViewBag.ErrorMessage = "Önce Bu Makineya Ait Fiyat Kaydını Silmelisiniz.";
            //        List<Makine> MakineListesi = dbContext.Makines.Include(x => x.YatakTip).Include(x => x.MakineTip).Include(x => x.MakineDurum).ToList();
            //        ViewBag.MakineDurumlari = dbContext.MakineDurums.ToList();
            //        ViewBag.YatakTipleri = dbContext.YatakTips.ToList();
            //        ViewBag.MakineTipleri = dbContext.MakineTips.ToList();
            //        return View("Index", MakineListesi);
            //    }
            //    if (fiyatcarpan != null)
            //    {
            //        ViewBag.ErrorMessage = "Önce Bu Makineya Ait Fiyat Çarpanı Kaydını Silmelisiniz.";
            //        List<Makine> MakineListesi = dbContext.Makines.Include(x => x.YatakTip).Include(x => x.MakineTip).Include(x => x.MakineDurum).ToList();
            //        ViewBag.MakineDurumlari = dbContext.MakineDurums.ToList();
            //        ViewBag.YatakTipleri = dbContext.YatakTips.ToList();
            //        ViewBag.MakineTipleri = dbContext.MakineTips.ToList();
            //        return View("Index", MakineListesi);
            //    }
            //    if (MakineKontenjan != null)
            //    {
            //        ViewBag.ErrorMessage = "Önce Bu Makineya Ait Kontenjan Kaydını Silmelisiniz.";
            //        List<Makine> MakineListesi = dbContext.Makines.Include(x => x.YatakTip).Include(x => x.MakineTip).Include(x => x.MakineDurum).ToList();
            //        ViewBag.MakineDurumlari = dbContext.MakineDurums.ToList();
            //        ViewBag.YatakTipleri = dbContext.YatakTips.ToList();
            //        ViewBag.MakineTipleri = dbContext.MakineTips.ToList();
            //        return View("Index", MakineListesi);
            //    }
            dbContext.Remove(makine);
            dbContext.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
