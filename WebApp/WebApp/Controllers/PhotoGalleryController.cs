using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WebApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PhotoGalleryController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PhotoGalleryController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

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

        ViewBag.FisImageId = null;

        //LAYOUT BİLDİRİM//
        List<Fotograf> FotografListesi = context.Fotografs.ToList();
        ViewBag.Isler = context.Jobs.ToList();
        return View(FotografListesi);
    }
    [HttpPost]
    public IActionResult FisImage(int id)
    {
        MakineContext context = new MakineContext();
        FisIslemleri fis = context.FisIslemleris.FirstOrDefault(f => f.Id == id);
        if (fis != null)
        {
            ViewBag.FisImageId = fis.IsId;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "fisImageId", fis.IsId);

        }
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
        var fisImageId = SessionHelper.GetObjectFromJson<long?>(HttpContext.Session, "fisImageId");
        ViewBag.FisImageId = fisImageId;

        //LAYOUT BİLDİRİM//
        List<Fotograf> FotografListesi = context.Fotografs.ToList();
        ViewBag.Isler = context.Jobs.ToList();

        return View("Index",FotografListesi);

       
    }


    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return RedirectToAction("Index");
            }
            MakineContext context = new MakineContext();
            Fotograf photo = new Fotograf();
            var fileName = Path.GetFileName(file.FileName);
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "WebAdminTheme", "img");
            var filePath = Path.Combine(imagePath, fileName);
            var DbImagePath = "/WebAdminTheme/img/";
            var dbfilePath = Path.Combine(DbImagePath, fileName);
            photo.Adi = fileName;
            photo.Yolu = dbfilePath;
            photo.Durumu = 1;
            photo.OlusturulmaTarihi = DateTime.Now;
            photo.GuncellenmeTarihi = DateTime.Now;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "photoVeriler", photo);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "fileVeriler", file);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("plusImage");
        }
        catch (Exception ex)
        {
            return BadRequest("Hata oluştu: " + ex.Message);
        }
    }
    [HttpPost]
    public IActionResult DeleteImage(int id)
    {
        try
        {
            var context = new MakineContext();
            var photo = context.Fotografs.FirstOrDefault(p => p.Id == id);
            if (id <= 0)
            {
                return BadRequest("Geçersiz fotoğraf kimliği.");
            }

            // Fotoğrafı veritabanından bulalım



            if (photo == null)
            {
                return NotFound("Fotoğraf bulunamadı.");
            }

            // Fotoğrafı img klasöründen sil
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "WebAdminTheme", "img", photo.Adi);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            // Fotoğrafı veritabanından sil
            context.Fotografs.Remove(photo);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return BadRequest("Hata oluştu: " + ex.Message);
        }
    }

    [HttpPost]
    public IActionResult plusImage(Fotograf photo)
    {
        MakineContext context = new MakineContext();
        Fotograf photoVeriler = SessionHelper.GetObjectFromJson<Fotograf>(HttpContext.Session, "photoVeriler");
        photoVeriler.IsId = photo.IsId;
        context.Fotografs.Add(photoVeriler);
        context.SaveChanges();

        return RedirectToAction("Index");
    }
}
