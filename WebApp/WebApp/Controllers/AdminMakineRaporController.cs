using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineRaporController : Controller
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
            List<Makine> MakineListesi = context.Makines.Include(x => x.MakineDurum).ToList();
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
    }
}
