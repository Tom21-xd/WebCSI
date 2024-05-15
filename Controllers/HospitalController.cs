using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;
using System.Data;
using WebCSI.Data;
using WebCSI.Models;
using WebReservas.Data;

namespace WebCSI.Controllers
{
    [Authorize]

    public class HospitalController : Controller
    {
        Conexion cn = new Conexion();
        ConexionMongo cm = new ConexionMongo();
        public IActionResult Index()
        {
            DataTable dt = cn.ProcedimientosSelect(null, "ListarHospi", null);
            List<HospitalModel> h = dt.DataTableToList<HospitalModel>();
            foreach (HospitalModel a in h)
            {
                a.Imagen = new ImagenModel();
                a.Imagen = cm.GetImage(a.IMAGEN_HOSPITAL);
            }

            return View(h);
        }
    }
}
