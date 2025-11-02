using Microsoft.AspNetCore.Mvc;

namespace Diplomados_VillaVocado.Controllers
{
    public class UsuariosWEbController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
