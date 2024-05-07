using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebCSI.Data;
using WebCSI.Models;

namespace WebCSI.Controllers
{
    public class UsuarioController : Controller
    {
        Conexion cn = new Conexion();
        public IActionResult Index()
        {
            DataTable usu = cn.ProcedimientosSelect(null, "ListarUsuarios", null);
            List<UsuarioModel> usuarios = usu.DataTableToList<UsuarioModel>();

            DataTable departamentos = cn.ProcedimientosSelect(null, "ListarDepartamento", null);
            List<DepartamentoModel> departamentosList = departamentos.DataTableToList<DepartamentoModel>();

            DataTable roles = cn.ProcedimientosSelect(null, "ListarRoles", null);
            List<RolModel> rolesList = roles.DataTableToList<RolModel>();

            DataTable tiposSangre = cn.ProcedimientosSelect(null, "ListarTipoSangre", null);
            List<TipoSangreModel> tiposSangreList = tiposSangre.DataTableToList<TipoSangreModel>(); 
            
            DataTable generos = cn.ProcedimientosSelect(null, "ListarGenero", null);
            List<GeneroModel> generosList = generos.DataTableToList<GeneroModel>();


            ViewBag.Departamentos = departamentosList;
            ViewBag.Roles = rolesList;
            ViewBag.TiposSangre = tiposSangreList;
            ViewBag.Generos = generosList;
            return View(usuarios);
        }

        public IActionResult filtrodeparta(string filtro)
        {
            string[] aux = { filtro };
            string[] parametros = { "filtro" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ListarMunicipio", aux);
            List<MunicipioModel> municipio = dt.DataTableToList<MunicipioModel>();

            return Json(municipio);
        }

        [HttpPost]
        public IActionResult crarUsuario(UsuarioModel a)
        {
            string[] parametros = { "nomu", "correou", "diru", "rolu", "muniu", "tiposangreu"  };
            string[] valores = { a.NOMBRE_USUARIO, a.CORREO_USUARIO, a.DIRECCION_USUARIO, a.FK_ID_ROL+"", a.FK_ID_MUNICIPIO.ToString(), a.FK_ID_TIPOSANGRE.ToString() };
            cn.procedimientosInEd(parametros, "InsertarUsuario", valores);

            return RedirectToAction("Index", "Usuario");
        }
    }
}
