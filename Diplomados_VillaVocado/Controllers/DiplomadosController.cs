using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diplomados_VillaVocado.Controllers
{
    public class DiplomadosController : Controller
    {
        private readonly IDiplomadoService _diplomadoService;
        private readonly IMateriaService _materiaService;

        public DiplomadosController(IDiplomadoService diplomadoService, IMateriaService materiaService)
        {
            _diplomadoService = diplomadoService;
            _materiaService = materiaService;
        }

        // GET: Diplomados/Disponibles - Para usuarios normales
        [HttpGet]
        public async Task<IActionResult> Disponibles()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            try
            {
                var diplomados = await _diplomadoService.GetDiplomadosDisponibles(usuarioId);
                return View(diplomados);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar diplomados: " + ex.Message;
                return View(new List<Diplomado>());
            }
        }

        // GET: Diplomados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var diplomado = await _diplomadoService.GetDiplomadoConDetalles(id.Value);
                if (diplomado == null)
                {
                    return NotFound();
                }

                return View(diplomado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar diplomado: " + ex.Message;
                return RedirectToAction(nameof(Disponibles));
            }
        }

        // POST: Diplomados/Inscribir/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribir(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (!usuarioId.HasValue)
            {
                TempData["Error"] = "Debe iniciar sesión para inscribirse";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _diplomadoService.InscribirUsuario(usuarioId.Value, id);
                TempData["Success"] = "Te has inscrito exitosamente al diplomado";
                return RedirectToAction(nameof(MisDiplomados));
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al inscribirse: " + ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Diplomados/MisDiplomados
        public async Task<IActionResult> MisDiplomados()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cargas = await _diplomadoService.GetCargasDiplomados(usuarioId.Value);
                return View(cargas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar tus diplomados: " + ex.Message;
                return View(new List<CargaDiplomado>());
            }
        }

        // POST: Diplomados/Cancelar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var resultado = await _diplomadoService.CancelarInscripcion(id, usuarioId.Value);
                if (resultado)
                {
                    TempData["Success"] = "Inscripción cancelada exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo cancelar la inscripción";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cancelar: " + ex.Message;
            }

            return RedirectToAction(nameof(MisDiplomados));
        }

        // Método helper para verificar autenticación
        private bool EsAdministrador()
        {
            var rol = HttpContext.Session.GetInt32("UsuarioRol");
            return rol == (int)RolUsuario.Administrador;
        }

        private bool EstaAutenticado()
        {
            return HttpContext.Session.GetInt32("UsuarioId") != null;
        }
    }
}
