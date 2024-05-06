using Microsoft.AspNetCore.Mvc;

using To_do_list.Models;
using To_do_list.Recursos;
using To_do_list.Servicios.Contrato;

//Referencias para trabajar con la autenticación por cookies
using System.Security.Claims; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace To_do_list.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;

        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario usuario)
        {
            usuario.Contrasena = Utilidades.EncriptarClave(usuario.Contrasena); //Encriptar la clave en formato SHA206

            Usuario usuarioCreado = await _usuarioServicio.SaveUsuario(usuario);

            if (usuarioCreado.Id > 0)
            {
                return RedirectToAction("IniciarSesion", "Inicio");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo rear el usuario";
            }

            return View();
        }


        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string contrasena)
        {
            Usuario usuarioEncontrado = await _usuarioServicio.GetUsuario(correo, Utilidades.EncriptarClave(contrasena)); //Recibir el correo y la contraseña pero con la contraseña encriptada

            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron conincidencias";
                return View();
            }

            //Objeto para almacenar la información del usuario
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioEncontrado.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
                new Claim(ClaimTypes.Surname, usuarioEncontrado.Apellido)
            };

            //Registrar el claims en una estructura por defecto
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Crear las propiedad de la autenticación
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true //Permitir que se refresque la autenticación
            };

            //Registrar como sesión iniciada a nuestro usuario
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Index", "Home");
        }
    }
}
