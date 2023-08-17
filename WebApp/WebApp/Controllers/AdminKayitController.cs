using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminKayitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(Kullanicilar user)
        {
            MakineContext context = new MakineContext();

            if (ModelState.IsValid)
            {
                if (context.Kullanicilars.Any(k => k.Adi == user.Adi && k.Id != user.Id))
                {
                    ViewBag.ErrorMessage = "Bu Kullanıcı Adı Zaten Kullanılıyor";
                    return View("Index");
                }

                context.Kullanicilars.Add(user);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index", user);
        }

    }
}
