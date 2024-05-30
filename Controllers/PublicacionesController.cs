using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Utilities;
using System.Data;
using WebCSI.Data;
using WebCSI.Models;
using WebReservas.Data;

namespace WebCSI.Controllers
{
    [Authorize]

    public class PublicacionesController : Controller
    {
        ConexionMongo mongo = new ConexionMongo();
        Conexion cn = new Conexion();
        public IActionResult Index()
        {
            DataTable dt = cn.ProcedimientosSelect(null, "ListarPublicaciones", null);
            List<PublicacionModel> lista = dt.DataTableToList<PublicacionModel>();
            foreach (PublicacionModel m in lista)
            {
                m.Imagen = new ImagenModel();
                m.Imagen = mongo.GetImage(m.IMAGEN_PUBLICACION);
            }

            return View(lista);
        }
        [HttpPost]
        public IActionResult crearPubli(PublicacionModel a)
       {
            ImagenModel img = new ImagenModel();
            img.Name = a.TITULO_PUBLICACION;
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
                        aux = mongo.UploadImage(img);
                    }
                }
            }
            a.IMAGEN_PUBLICACION = aux;

            string[] para = { "titulo", "idi", "descri", "usua" };
            string[] valo= {a.TITULO_PUBLICACION,a.IMAGEN_PUBLICACION,a.DESCRIPCION_PUBLICACION,User.Identity.Name};

            cn.procedimientosInEd(para, "CrearPublicacion", valo);

            return RedirectToAction("Index","Publicaciones");
        }

        public IActionResult Filtrar(string filtro)
        {
            if(filtro == null)
            {
                filtro = "";
            }
            string[] para = { "nombre" };
            string[] valo = { filtro };
            DataTable dt= cn.ProcedimientosSelect(para, "Filtrarpublicacion", valo);
            List<PublicacionModel> lista = dt.DataTableToList<PublicacionModel>();

            foreach (PublicacionModel m in lista)
            {
                m.Imagen = new ImagenModel();
                m.Imagen = mongo.GetImage(m.IMAGEN_PUBLICACION);
            }
            return Json(lista);
        }

        public IActionResult ObtenerPubli(int idp)
        {
            string[] para = { "idp" };
            string[] valo = { idp.ToString() };
            DataTable dt = cn.ProcedimientosSelect(para, "ObtenerPublicacion", valo);
            List<PublicacionModel> lista = dt.DataTableToList<PublicacionModel>();
            foreach (PublicacionModel m in lista)
            {
                m.Imagen = new ImagenModel();
                m.Imagen = mongo.GetImage(m.IMAGEN_PUBLICACION);
            }
            return View(lista[0]);
        }

        [HttpPost]
        public IActionResult EditarPubli(PublicacionModel a)
        {
            if(a.File != null)
            {
                ImagenModel img = new ImagenModel();
                img.Name = a.TITULO_PUBLICACION;
                byte[] bytes;
                using (Stream fs = a.File.OpenReadStream())
                {
                    using (BinaryReader br = new(fs))
                    {
                        bytes = br.ReadBytes((int)fs.Length);
                        img.Imagen = Convert.ToBase64String(bytes, 0, bytes.Length);
                        mongo.UpdateImage(img, a.IMAGEN_PUBLICACION);
                    }
                }
            }

            string[] para = { "idp", "titulo", "descri" };
            string[] valo = { a.ID_PUBLICACION.ToString(), a.TITULO_PUBLICACION, a.DESCRIPCION_PUBLICACION };

            cn.procedimientosInEd(para, "EditarPublicacion", valo);

            return RedirectToAction("Index", "Publicaciones");
        }
    }
}
