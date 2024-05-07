using Microsoft.AspNetCore.Mvc;

namespace WebCSI.Controllers
{
    public class PublicacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
