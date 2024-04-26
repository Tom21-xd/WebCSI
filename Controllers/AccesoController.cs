using Microsoft.AspNetCore.Mvc;

namespace WebCSI.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult Recuperar()
        {
            return View();
        }
    }
}
