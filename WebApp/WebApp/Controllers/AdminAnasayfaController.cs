using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApp.Models;

namespace WebApp.Controllers
{
   
    public class AdminAnasayfaController : Controller
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
           
            List<Is> IsListesi = context.Jobs.ToList();
            foreach (var Is in IsListesi)
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
            }
            var musteriListesi= context.Musteris.ToList();
            ViewBag.Musteriler = musteriListesi;
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



            foreach (var Is in IsListesi)
            {
                DateTime? tarihVeri = Is.BaslangicTarihi;
                TimeSpan KalanZamanVeri = (TimeSpan)(tarihVeri.Value.Date - DateTime.Now.Date);
                var ayBazliIsVerileri = IsListesi
     .GroupBy(job => job.BaslangicTarihi.Value.Month)
     .Select(group => new AyBazliIsVerisi { Ay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key), IsSayisi = group.Count() })
     .OrderBy(veri => DateTime.ParseExact(veri.Ay, "MMMM", CultureInfo.CurrentCulture))
     .Select(veri => new AyBazliIsVerisi { Ay = veri.Ay.Substring(0, 3).ToUpper(), IsSayisi = veri.IsSayisi }) // Ay verisini düzenle
     .ToList();



                // Ay bazlı iş verilerini Model'e aktar
                ViewBag.AyBazliIsVerileri = ayBazliIsVerileri;

                //ay adları 
                CultureInfo trCulture = new CultureInfo("tr-TR");
                List<string> turkceAyAdlari = new List<string>();

                for (int i = 1; i <= 12; i++)
                {
                    string ayAdi = trCulture.DateTimeFormat.GetMonthName(i);
                    string ayAdiKisaltma = ayAdi.Substring(0, 3).ToUpper();

                    string SimdikiAy = DateTime.Now.ToString("MMMM", trCulture);
                    string SimdikiAyKisaltma= SimdikiAy.Substring(0, 3).ToUpper();
                    //AYLIK KAZANÇ KISMI BAŞLANGIÇ
                    if (ayAdiKisaltma== SimdikiAyKisaltma)
                    {
                        int aySirasi = DateTime.Now.Month;
                        var toplamAylikKazanc=context.FisIslemleris.Where(Is=>Is.BitisTarihi.Value.Month==aySirasi).ToList();
                        ViewBag.toplamAylikKazanc = toplamAylikKazanc.Sum(Is => Is.Ucret);
                    //AYLIK KAZANÇ KISMI BİTİŞ
                    }
                    turkceAyAdlari.Add(ayAdiKisaltma);
                }
                //ay adlarını modele aktar
                ViewBag.TurkceAyAdlari = turkceAyAdlari;

                
            }
            ViewBag.ClosestJob = null;
            ViewBag.Is = IsListesi;

            //İSTATİSLİKLER BAŞLANGIÇ
            var fisIslemleri = context.FisIslemleris.ToList();
            var kullanicilar = context.Kullanicilars.ToList();
            var toplamUcret = fisIslemleri.Sum(fis => fis.Ucret);
            
            ViewBag.toplamIs = IsListesi.Count();
            ViewBag.toplamKullanici = kullanicilar.Count();
            
            
            ViewBag.toplamUcret= (int)toplamUcret;
            ViewBag.toplamMusteri = musteriListesi.Count();
            //İSTATİSLİKLER BİTİŞ
            context.SaveChanges();
            return View(IsListesi);
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlyData()
        {
            var currentMonth = DateTime.Now.Month;
            var monthlyData = await context.FisIslemleris
                .Where(f => f.BitisTarihi.Value.Month == currentMonth)
                .Select(f => new
                {
                    Ucret = f.Ucret,
                    BitisGun = f.BitisTarihi.Value.Day,
                    BitisTarih = f.BitisTarihi
                    
                })
                .ToListAsync();
           
            return Json(monthlyData);
        }

        [HttpGet]
        public async Task<IActionResult> PercentageData()
        {


            var currentMonth = DateTime.Now.Month;
            var currentMonthlyData = await context.FisIslemleris
                .Where(f => f.BitisTarihi.Value.Month == currentMonth)
                .Select(f => new
                {
                    Ucret = f.Ucret,
                    BitisGun = f.BitisTarihi.Value.Day,
                    BitisTarih = f.BitisTarihi

                })
                .ToListAsync();

            var lastMonth = DateTime.Now.Month - 1;
            var lastMonthlyData = await context.FisIslemleris
                .Where(f => f.BitisTarihi.Value.Month == lastMonth)
                .Select(f => new
                {
                    Ucret = f.Ucret,
                    BitisGun = f.BitisTarihi.Value.Day,
                    BitisTarih = f.BitisTarihi

                })
                .ToListAsync();

            
                //şuanki ay verileri
                var currentYapilanİsSayisi = context.FisIslemleris.Where(f => f.BitisTarihi.Value.Month == currentMonth).Count();
                var currentMusteriSayisi = context.Musteris.Where(f => f.OlusturulmaTarihi.Value.Month == currentMonth).Count();
                var currentToplamUcret = Convert.ToInt32(currentMonthlyData.Sum(u => u.Ucret));
                //geçtiğimiz ay verileri 
                var lastYapilanİsSayisi = context.FisIslemleris.Where(f => f.BitisTarihi.Value.Month == lastMonth).Count();
                var lastMusteriSayisi = context.Musteris.Where(f => f.OlusturulmaTarihi.Value.Month == lastMonth).Count();
                var lastToplamUcret = Convert.ToInt32(lastMonthlyData.Sum(u => u.Ucret));


            var currentVeriler = new List<object> { currentYapilanİsSayisi, currentMusteriSayisi, currentToplamUcret };
            var lastVeriler = new List<object> { lastYapilanİsSayisi, lastMusteriSayisi, lastToplamUcret };

            var ayToplamVeriler = new List<List<object>> { currentVeriler, lastVeriler};




            //var farkYuzdesi = ((currentToplamUcret - lastToplamUcret) / lastToplamUcret) * 100;




            return Json(ayToplamVeriler);
        }
    }
}
