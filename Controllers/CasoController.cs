using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebCSI.Data;
using WebCSI.Models;

namespace WebCSI.Controllers
{
    [Authorize]

    public class CasoController : Controller
    {
        Conexion cn = new Conexion();
        public IActionResult Index()
        {
            DataTable dt = cn.ProcedimientosSelect(null, "ListarCasos", null);
            List<CasoModel> lista = dt.DataTableToList<CasoModel>();

            dt=cn.ProcedimientosSelect(null, "ListarSintomas", null);
            List<SintomaModel> sintomas = dt.DataTableToList<SintomaModel>();

            dt = cn.ProcedimientosSelect(null, "ListarTIpoDengue", null);
            List<TipoDengueModel> tipoDengue = dt.DataTableToList<TipoDengueModel>();

            dt=cn.ProcedimientosSelect(null, "ListarDepartamento", null);
            List<DepartamentoModel> departamento = dt.DataTableToList<DepartamentoModel>();

            dt= cn.ProcedimientosSelect(null, "ListarMuni", null);
            List<MunicipioModel> muni = dt.DataTableToList<MunicipioModel>();

            dt = cn.ProcedimientosSelect(null, "ListarHospi", null);
            List<HospitalModel> hospital = dt.DataTableToList<HospitalModel>();

            dt=cn.ProcedimientosSelect(null, "ListarEstadocaso", null);
            List<EstadoCasoModel> estado = dt.DataTableToList<EstadoCasoModel>();

            ViewBag.estado = estado;
            ViewBag.muni = muni;
            ViewBag.hospital = hospital;
            ViewBag.departamento = departamento;
            ViewBag.tipoDengue = tipoDengue;
            ViewBag.sintomas = sintomas;
            return View(lista);
        }
        [HttpPost]
        public IActionResult filtrarUsua(string filtro)
        {
            string[] aux= { filtro };
            string[] parametros = { "filtro" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "Filtrar", aux);
            List<UsuarioModel> lista = dt.DataTableToList<UsuarioModel>();
            return Json(lista);
        }
        [HttpPost]
        public IActionResult  filtromuni (string filtro) 
        {
            string[] aux = { filtro };
            string[] parametros = { "filtro" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ListarMunicipio", aux);
            List<MunicipioModel> municipio = dt.DataTableToList<MunicipioModel>();

            return Json(municipio);
        }

        [HttpPost] 
        public IActionResult filtrarHospital(string filtro)
        {
            string[] aux = { filtro };
            string[] parametros = { "filtro" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ListarHospital", aux);
            List<HospitalModel> hospital = dt.DataTableToList<HospitalModel>();

            return Json(hospital);
        }

        public IActionResult crearcaso(CasoModel a)
        {
            a.FK_ID_PERSONALMEDICO = Convert.ToInt32(User.Identity.Name+"");
            string[] parametros = { "descri", "ihospital", "tdengue", "paciente", "personalmedico", "direccion"};
            string[] datos = { a.DESCRIPCION_CASOREPORTADO, a.FK_ID_HOSPITAL+"", a.FK_ID_TIPODENGUE+"", a.FK_ID_PACIENTE+"", a.FK_ID_PERSONALMEDICO+"",a.DIRECCION_CASOREPORTADO };
            cn.procedimientosInEd(parametros, "CrearCaso", datos);  
            return RedirectToAction("Index","Caso");
        }

        [HttpPost]
        public IActionResult editar(string id)
        {
            string[] aux = { id };
            string[] parametros = { "idc" };
            DataTable dataTable = cn.ProcedimientosSelect(parametros, "ObtenerCaso", aux);
            List<CasoModel> lista = dataTable.DataTableToList<CasoModel>();
            return Json(lista[0]);
        }

        [HttpPost]
        public IActionResult actualizar(CasoModel a)
        {
            string[] parametros = { "idc", "descri","estadoc", "ihospital", "tdengue" };
            string[] datos = { a.ID_CASOREPORTADO+"", a.DESCRIPCION_CASOREPORTADO, a.FK_ID_ESTADOCASO+"",a.FK_ID_HOSPITAL+"", a.FK_ID_TIPODENGUE+""};
            cn.procedimientosInEd(parametros, "ActualizarCaso", datos);
            return RedirectToAction("Index", "Caso");
        }

        public IActionResult mapaDeCalor()
        {
            DataTable dt = cn.ProcedimientosSelect(null, "ListarCasos", null);
            List<CasoModel> l1 = dt.DataTableToList<CasoModel>();

            DataTable dt1 = cn.ProcedimientosSelect(null, "CantidadCasoHospital", null);
            List<HospitalModel> lista = dt1.DataTableToList<HospitalModel>();

            ViewBag.lista = l1;
            return View(lista);
        }
        public IActionResult mapPopup()
        {
            return View();  
        }

    }
}
