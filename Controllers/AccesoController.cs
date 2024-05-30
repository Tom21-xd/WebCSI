using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebCSI.Models;
using WebCSI.Data;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using WebCSI.Helper;

namespace WebCSI.Controllers
{
    public class AccesoController : Controller
    {
        Conexion cn = new Conexion();

        public IActionResult Login()
            {
                if(User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Usuario");
                }   
                return View();
            }

        public IActionResult Recuperar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Validar(String correo, String contra)
        {

            string[] datos = { correo, contra };
            string[] parametros = { "correo", "contra" };

            DataTable usu = cn.ProcedimientosSelect(parametros,"ValidarUsuario",datos);
            List<UsuarioModel> usuarios = usu.DataTableToList<UsuarioModel>();
            if (usuarios.Count != 0)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarios[0].ID_USUARIO+""),
                    new Claim(ClaimTypes.NameIdentifier, usuarios[0].NOMBRE_USUARIO),   
                    new Claim(ClaimTypes.Email, usuarios[0].CORREO_USUARIO),
                    new Claim(ClaimTypes.Actor, usuarios[0].FK_ID_ROL+""),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                 
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return RedirectToAction("Login","Acceso");
        }

        public async Task<IActionResult> salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        [HttpPost]
        public ActionResult RecuContra(string correo)
        {
            ServicioGmail aux = new ServicioGmail();
            String nuevac = generarclave();
            string[] datos = { correo, nuevac };
            string[] parametros = { "correo", "contra" };
            cn.procedimientosInEd(parametros, "RecuperarContra", datos);
            aux.SendEmailGmail(correo, "Recuperacion De Contraseña", "Su nueva Contraseña es :" + nuevac);
            return View("Login");
        }

        public String generarclave()
        {
            Random aleatorio = new Random();
            string conjuntoCaracteres = "abcdefghijklmnopqrstuvwxyz0123456789";
            string cadena = "";
            for (int i = 0; i < 6; i++)
            {
                cadena += conjuntoCaracteres[aleatorio.Next(conjuntoCaracteres.Length)];
            }
            return cadena;
        }
    }
}
