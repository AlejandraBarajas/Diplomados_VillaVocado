using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diplomados_VillaVocado.Controllers
{
    public class AccountController: Controller
    {
        private readonly IUsuarioService _usuarioService;
        public AccountController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está logeado, redirigir
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToHome();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string password)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Correo y contraseña son requeridos";
                return View();
            }
            try
            {
                var usuario = await _usuarioService.Login(correo, password);

                if (usuario == null)
                {
                    ViewBag.Error = "Correo o contraseña incorrectos";
                    return View();
                }
                // Guardar en sesión
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetInt32("UsuarioRol", (int)usuario.Rol);
                TempData["Success"] = $"¡Bienvenido, {usuario.Nombre}!";
                // Redirigir según el rol
                if (usuario.Rol == RolUsuario.Administrador)
                {
                    return RedirectToAction("Index", "Usuarios");
                } else {
                    return RedirectToAction("Disponibles", "Diplomados"); }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al iniciar sesión: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Si ya está logeado, redirigir
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToHome();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string nombre, string correo, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Todos los campos son requeridos";
                return View();
            }
            if (password != confirmPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            if (password.Length < 6)
            {
                ViewBag.Error = "La contraseña debe tener al menos 6 caracteres";
                return View();
            }
            try
            {
                await _usuarioService.Register(nombre, correo, password);

                TempData["Success"] = "Usuario registrado exitosamente. Ahora puedes iniciar sesión.";
                return RedirectToAction(nameof(Login));
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al registrar usuario: " + ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            try
            {
                // Limpiar toda la sesión
                HttpContext.Session.Clear();

                // Mensaje de éxito
                TempData["Success"] = "Has cerrado sesión exitosamente";

                // Redirigir al login
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cerrar sesión: " + ex.Message;
                return RedirectToAction("Login", "Account");
            }
        }

        private IActionResult RedirectToHome()
        {
            var rol = HttpContext.Session.GetInt32("UsuarioRol");
            if (rol == (int)RolUsuario.Administrador)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            return RedirectToAction("Disponibles", "Diplomados");
        }
    }
}
