using Microsoft.AspNetCore.Mvc;

namespace WebCSI.Controllers
{
    public class HospitalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
