using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
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
            string[] aux = { "" };
            string[] parametros = { "nombre" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "FiltrarHospital", aux);
            List<HospitalModel> h = dt.DataTableToList<HospitalModel>();
            foreach (HospitalModel a in h)
            {
                a.Imagen = new ImagenModel();
                a.Imagen = cm.GetImage(a.IMAGEN_HOSPITAL);
            }
            DataTable departamentos = cn.ProcedimientosSelect(null, "ListarDepartamento", null);
            List<DepartamentoModel> departamentosList = departamentos.DataTableToList<DepartamentoModel>();

            DataTable muni = cn.ProcedimientosSelect(null, "ListarMuni", null);
            List<MunicipioModel> lmuni = muni.DataTableToList<MunicipioModel>();

            ViewBag.muni = lmuni;
            ViewBag.Departamentos = departamentosList;

            return View(h);
        }

        [HttpPost]
        public IActionResult filtraHospi(string filtro)
        {
            if (filtro == null)
            {
                filtro = "";
            }
            string[] aux = { filtro };
            string[] parametros = { "nombre" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "FiltrarHospital", aux);
            List<HospitalModel> h = dt.DataTableToList<HospitalModel>();
            foreach (HospitalModel a in h)
            {
                a.Imagen = new ImagenModel();
                a.Imagen = cm.GetImage(a.IMAGEN_HOSPITAL);
            }

            return Json(h);
        }
        [HttpPost]
        public IActionResult filtrodeparta(string filtro)
        {
            string[] aux = { filtro };
            string[] parametros = { "filtro" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ListarMunicipio", aux);
            List<MunicipioModel> municipio = dt.DataTableToList<MunicipioModel>();

            return Json(municipio);
        }

        [HttpPost]
        public IActionResult crarHospital(HospitalModel a)
        {
            ImagenModel img = new ImagenModel();
            img.Name = a.NOMBRE_HOSPITAL;
            string aux = "";
            byte[] bytes;
            if (a.File != null)
            {
                using (Stream fs = a.File.OpenReadStream())
                {
                    using (BinaryReader br = new(fs))
                    {
                        bytes = br.ReadBytes((int)fs.Length);
                        img.Imagen = Convert.ToBase64String(bytes, 0, bytes.Length);
                        aux = cm.UploadImage(img);
                    }
                }
            }
            a.IMAGEN_HOSPITAL = aux;

            string cor = Request.Form["ubi"];
            string[] cor1 = cor.Split(":");

            string[] parametros = { "nombre", "direccion", "lat", "lon", "muni", "imagen" };
            string[] valores = { a.NOMBRE_HOSPITAL, a.DIRECCION_HOSPITAL, cor1[0], cor1[1], a.FK_ID_MUNICIPIO + "", a.IMAGEN_HOSPITAL };
            cn.procedimientosInEd(parametros, "CrearHospital", valores);

            return RedirectToAction("Index", "Hospital");
        }

        [HttpPost]
        public IActionResult ObtenerHospital(int id)
        {
            string[] aux = { id.ToString() };
            string[] parametros = { "idh" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ObtenerHospital", aux);
            List<HospitalModel> h = dt.DataTableToList<HospitalModel>();
            return Json(h[0]);
        }

        [HttpPost]
        public IActionResult EditarHospital(HospitalModel a)
        {
            string[] parametros = { "idh", "nombre", "imagenN" };
            if(a.File != null)
            {
                ImagenModel img = new ImagenModel();
                img.Name = a.NOMBRE_HOSPITAL;
                byte[] bytes;
                using (Stream fs = a.File.OpenReadStream())
                {
                    using (BinaryReader br = new(fs))
                    {
                        bytes = br.ReadBytes((int)fs.Length);
                        img.Imagen = Convert.ToBase64String(bytes, 0, bytes.Length);
                        cm.UpdateImage(img,a.IMAGEN_HOSPITAL);
                    }
                }
            }
            string[] valores = { a.ID_HOSPITAL.ToString(), a.NOMBRE_HOSPITAL, a.DIRECCION_HOSPITAL, a.IMAGEN_HOSPITAL+"" };
            cn.procedimientosInEd(parametros, "EditarHospital", valores);

            return RedirectToAction("Index", "Hospital");
        }

        public IActionResult masInfo(int idh) {
            string[] aux = { idh.ToString() };
            string[] parametros = { "idh" };
            DataTable dt = cn.ProcedimientosSelect(parametros, "ObtenerHospital", aux);
            List<HospitalModel> h = dt.DataTableToList<HospitalModel>();
            foreach (HospitalModel a in h)
            {
                a.Imagen = new ImagenModel();
                a.Imagen = cm.GetImage(a.IMAGEN_HOSPITAL);
            }

            dt = cn.ProcedimientosSelect(parametros, "CasosXHospital", aux);
            List<CasoModel> c = dt.DataTableToList<CasoModel>();

            ViewBag.casos = c;
            return View(h[0]);
        }

    }


}
