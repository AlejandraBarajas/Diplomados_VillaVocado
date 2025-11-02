using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diplomados_VillaVocado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Correo y contraseña son requeridos" });
            }
            try
            {
                var usuario = await _usuarioService.Login(request.Correo, request.Password);
                if (usuario == null)
                {
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetInt32("UsuarioRol", (int)usuario.Rol);

                return Ok(new
                {
                    message = "Login exitoso",
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Rol
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al iniciar sesión", error = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Sesión cerrada exitosamente" });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<object>> Register([FromBody] RegisterRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { message = "Nombre requerido" });
            if (string.IsNullOrWhiteSpace(request.Correo))
                return BadRequest(new { message = "Correo requerido" });
            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Contraseña requerida" });
            try
            {
                var usuario = await _usuarioService.Register(request.Nombre, request.Correo, request.Password);
                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.Rol,
                    message = "Nuevo Usuario registrado"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar usuario", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuarios()
        {
            if (!EsAdministrador()) //validacion del tipo de usuario
                return Forbid();
            try
            {
                var usuarios = await _usuarioService.GetAllUsuarios();
                var resultado = usuarios.Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Correo,
                    u.Rol,
                    u.Activo,
                    u.CreatedAt,
                    DiplomadosInscritos = u.Diplomados.Count
                });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener usuarios", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioConDiplomados(id);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }
                return Ok(new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.Rol,
                    usuario.Activo,
                    usuario.CreatedAt,
                    Diplomados = usuario.Diplomados.Select(cd => new
                    {
                        cd.Id,
                        cd.DiplomadoId,
                        DiplomadoNombre = cd.Diplomado.Nombre,
                        cd.FechaInscripcion,
                        cd.Estado,
                        FechaInicio = cd.Diplomado.FechaInicio,
                        FechaFin = cd.Diplomado.FechaFin,
                        CantidadMaterias = cd.Diplomado.Materias.Count
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener usuario", error = ex.Message });
            }
        }

        [HttpGet("MisDiplomados")]
        public async Task<ActionResult<object>> GetMisDiplomados()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return Unauthorized(new { message = "Debe iniciar sesión" });
            try
            {
                var diplomados = await _usuarioService.GetDiplomadosDeUsuario(usuarioId.Value);
                return Ok(diplomados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener diplomados", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UpdateUsuarioRequest request)
        {
            if (!EsAdministrador())
                return Forbid();
            try
            {
                await _usuarioService.UpdateUsuario(
                    id,
                    request.Nombre,
                    request.Correo,
                    request.Rol,
                    request.Activo
                );

                return Ok(new { message = "Usuario actualizado exitosamente" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar usuario", error = ex.Message });
            }
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (!EsAdministrador())
                return Forbid();
            try
            {
                var userDeleteId = HttpContext.Session.GetInt32("UsuarioId");
                var resultado = await _usuarioService.DeleteUsuario(id, userDeleteId);

                if (!resultado)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(new { message = "Usuario eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar usuario", error = ex.Message });
            }
        }

        private bool EsAdministrador()
        {
            var rol = HttpContext.Session.GetInt32("UsuarioRol");
            return rol == (int)RolUsuario.Administrador;
        }
    }
    public class LoginRequest
    {
        public required string Correo { get; set; }
        public required string Password { get; set; }
    }
    public class RegisterRequest
    {
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public required string Password { get; set; }
    }
    public class UpdateUsuarioRequest
    {
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public RolUsuario? Rol { get; set; }
        public bool? Activo { get; set; }
    }
}

