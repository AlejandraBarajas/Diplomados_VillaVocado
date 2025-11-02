using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diplomados_VillaVocado.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Helper para verificar si es admin
        private bool EsAdministrador()
        {
            var rol = HttpContext.Session.GetInt32("UsuarioRol");
            return rol == (int)RolUsuario.Administrador;
        }

        // Helper para verificar login
        private bool EstaAutenticado()
        {
            return HttpContext.Session.GetInt32("UsuarioId") != null;
        }

        // ========================================
        // GET: /Usuarios
        // ========================================
        public async Task<IActionResult> Index()
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                var usuarios = await _usuarioService.GetAllUsuarios();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar usuarios: " + ex.Message;
                return View(new List<Usuario>());
            }
        }

        // ========================================
        // GET: /Usuarios/Details/5
        // ========================================
        public async Task<IActionResult> Details(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                var usuario = await _usuarioService.GetUsuarioConDiplomados(id);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar usuario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ========================================
        // GET: /Usuarios/Create
        // ========================================
        public IActionResult Create()
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            return View();
        }

        // ========================================
        // POST: /Usuarios/Create
        // ========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nombre, string correo, string password, string rol)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Todos los campos son requeridos";
                return View();
            }

            try
            {
                var usuario = await _usuarioService.Register(nombre, correo, password);

                // Actualizar rol si es diferente de Usuario
                if (!string.IsNullOrEmpty(rol) && Enum.TryParse<RolUsuario>(rol, out var rolEnum))
                {
                    await _usuarioService.UpdateUsuario(usuario.Id, null, null, rolEnum, null);
                }

                TempData["Success"] = "Usuario creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear usuario: " + ex.Message;
                return View();
            }
        }

        // ========================================
        // GET: /Usuarios/Edit/5
        // ========================================
        public async Task<IActionResult> Edit(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                var usuario = await _usuarioService.GetUsuarioById(id);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar usuario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ========================================
        // POST: /Usuarios/Edit/5
        // ========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string nombre, string correo, string rol, bool activo)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                RolUsuario? rolEnum = null;
                if (!string.IsNullOrEmpty(rol) && Enum.TryParse<RolUsuario>(rol, out var parsedRol))
                {
                    rolEnum = parsedRol;
                }

                await _usuarioService.UpdateUsuario(id, nombre, correo, rolEnum, activo);

                TempData["Success"] = "Usuario actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                var usuario = await _usuarioService.GetUsuarioById(id);
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar usuario: " + ex.Message;
                var usuario = await _usuarioService.GetUsuarioById(id);
                return View(usuario);
            }
        }

        // ========================================
        // GET: /Usuarios/Delete/5
        // ========================================
        public async Task<IActionResult> Delete(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                var usuario = await _usuarioService.GetUsuarioById(id);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar usuario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ========================================
        // POST: /Usuarios/Delete/5
        // ========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                var userDeleteId = HttpContext.Session.GetInt32("UsuarioId");
                var resultado = await _usuarioService.DeleteUsuario(id, userDeleteId);

                if (!resultado)
                {
                    TempData["Error"] = "Usuario no encontrado";
                }
                else
                {
                    TempData["Success"] = "Usuario eliminado exitosamente";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar usuario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
