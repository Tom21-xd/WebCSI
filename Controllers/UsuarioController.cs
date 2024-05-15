using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebCSI.Data;
using WebCSI.Models;

namespace WebCSI.Controllers
{
    [Authorize]

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

            DataTable muni = cn.ProcedimientosSelect(null, "ListarMuni",null);
            List<MunicipioModel> lmuni = muni.DataTableToList<MunicipioModel>();



            ViewBag.muni = lmuni;
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
            string[] parametros = { "nomu", "correou", "diru", "rolu", "muniu", "tiposangreu" ,"genu" };
            string[] valores = { a.NOMBRE_USUARIO, a.CORREO_USUARIO, a.DIRECCION_USUARIO, a.FK_ID_ROL+"", a.FK_ID_MUNICIPIO.ToString(), a.FK_ID_TIPOSANGRE.ToString(),a.FK_ID_GENERO+""};
            cn.procedimientosInEd(parametros, "CrearUsuario", valores);

            return RedirectToAction("Index", "Usuario");
        }

        public IActionResult HistorialCasos(int id)
        {
            string[] parametros = { "idp" };
            string[] valores = { id.ToString() };
            DataTable casos = cn.ProcedimientosSelect(parametros, "HistorialCasos", valores);
            List<CasoModel> casosList = casos.DataTableToList<CasoModel>();

            return View(casosList);
        }

        [HttpPost]
        public IActionResult ObtenerUSua(int id)
        {
            string[] aux = { id.ToString() };
            string[] parametros = { "idu" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ObtenerUsuario", aux);
            List<UsuarioModel> usuario = dt.DataTableToList<UsuarioModel>();
            return Json(usuario[0]);
        }

        [HttpPost]
        public IActionResult EditarUsuario(UsuarioModel a)
        {
            string[] parametros = { "idu", "nombre", "correo", "dire", "rolu", "muni", "gene" };
            string[] valores = { a.ID_USUARIO.ToString(), a.NOMBRE_USUARIO, a.CORREO_USUARIO, a.DIRECCION_USUARIO, a.FK_ID_ROL + "", a.FK_ID_MUNICIPIO.ToString(), a.FK_ID_GENERO + "" };
            cn.procedimientosInEd(parametros, "EditarUsuario", valores);

            return RedirectToAction("Index", "Usuario");
        }   

    }
}
