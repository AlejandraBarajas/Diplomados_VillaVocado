using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diplomados_VillaVocado.Controllers
{
    public class MateriasController : Controller
    {
        private readonly IMateriaService _materiaService;
        private readonly IDiplomadoService _diplomadoService;
        public MateriasController(IMateriaService materiaService, IDiplomadoService diplomadoService)
        {
            _materiaService = materiaService;
            _diplomadoService = diplomadoService;
        }
        // Helper para verificar autenticación
        private bool EsAdministrador()
        {
            var rol = HttpContext.Session.GetInt32("UsuarioRol");
            return rol == (int)RolUsuario.Administrador;
        }
        private bool EstaAutenticado()
        {
            return HttpContext.Session.GetInt32("UsuarioId") != null;
        }

        public async Task<IActionResult> Index()
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");
            if (!EsAdministrador())
                return Forbid();
            try
            {
                var materias = await _materiaService.GetAllMaterias();
                return View(materias);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar materias: " + ex.Message;
                return View(new List<Materia>());
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");
            if (!EsAdministrador())
                return Forbid();
            try
            {
                var materia = await _materiaService.GetMateriaById(id);
                if (materia == null)
                {
                    TempData["Error"] = "Materia no encontrada";
                    return RedirectToAction(nameof(Index));
                }
                return View(materia);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar materia: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Create()
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");
            if (!EsAdministrador())
                return Forbid();
            try
            {
                // Cargar lista de diplomados para el dropdown
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar formulario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nombre, string descripcion, int orden, int duracionHoras, int diplomadoId)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");
            if (!EsAdministrador())
                return Forbid();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                ViewBag.Error = "El nombre es requerido";
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre", diplomadoId);
                return View();
            }
            try
            {
                var userCreateId = HttpContext.Session.GetInt32("UsuarioId");
                await _materiaService.CreateMateria(nombre, descripcion, orden, duracionHoras, diplomadoId, userCreateId);

                TempData["Success"] = "Materia creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre", diplomadoId);
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear materia: " + ex.Message;
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre", diplomadoId);
                return View();
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");
            if (!EsAdministrador())
                return Forbid();
            try
            {
                var materia = await _materiaService.GetMateriaById(id);
                if (materia == null)
                {
                    TempData["Error"] = "Materia no encontrada";
                    return RedirectToAction(nameof(Index));
                }
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre", materia.DiplomadoId);

                return View(materia);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar materia: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string nombre, string descripcion, int orden, int duracionHoras, bool activo)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                await _materiaService.UpdateMateria(id, nombre, descripcion, orden, duracionHoras, activo);

                TempData["Success"] = "Materia actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Materia no encontrada";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                var materia = await _materiaService.GetMateriaById(id);
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre", materia?.DiplomadoId);
                return View(materia);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar materia: " + ex.Message;
                var materia = await _materiaService.GetMateriaById(id);
                var diplomados = await _diplomadoService.GetAllDiplomados();
                ViewBag.Diplomados = new SelectList(diplomados, "Id", "Nombre", materia?.DiplomadoId);
                return View(materia);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Account");

            if (!EsAdministrador())
                return Forbid();

            try
            {
                var materia = await _materiaService.GetMateriaById(id);

                if (materia == null)
                {
                    TempData["Error"] = "Materia no encontrada";
                    return RedirectToAction(nameof(Index));
                }

                return View(materia);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar materia: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

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
                var resultado = await _materiaService.DeleteMateria(id, userDeleteId);

                if (!resultado)
                {
                    TempData["Error"] = "Materia no encontrada";
                }
                else
                {
                    TempData["Success"] = "Materia eliminada exitosamente";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar materia: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
