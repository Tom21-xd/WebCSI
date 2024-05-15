using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebCSI.Models;
using WebCSI.Data;
using System.Data;
using Microsoft.AspNetCore.Authorization;

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
    }
}
