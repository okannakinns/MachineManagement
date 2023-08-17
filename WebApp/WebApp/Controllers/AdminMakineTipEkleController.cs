using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminMakineTipEkleController : Controller
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
            List<MakineTip> makineTipListesi = context.MakineTips.ToList();

            return View(makineTipListesi);
        }

        [HttpPost]
        public IActionResult Ekle(MakineTip makinetip)
        {
            
                makinetip.OlusturulmaTarihi = DateTime.Now;
                makinetip.Durumu = 1;
                makinetip.GuncellenmeTarihi = DateTime.Now;
                MakineContext context = new MakineContext();
                if (context.MakineTips.Any(k => k.Adi == makinetip.Adi && k.Id != makinetip.Id))
                {
                    ViewBag.ErrorMessage = "Bu makine Tipi Zaten Kayıtlı.";
                    List<MakineTip> makinetipListesi = context.MakineTips.ToList();
                    return View("Index", makinetipListesi);
                }
                //if (makinetip.Adi == null)
                //{
                //    ViewBag.ErrorMessage = " makine Tip Adı Boş Bırakılamaz.";
                //    List<MakineTip> makinetipListesi = context.MakineTips.ToList();
                //    return View("Index", makinetipListesi);
                //}
                context.MakineTips.Add(makinetip);
                context.SaveChanges();

                return RedirectToAction("Index", "AdminMakineTip");
            }
        }
    }

