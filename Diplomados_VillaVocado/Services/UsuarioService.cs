using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplomados_VillaVocado.Services
{
    public class UsuarioService: IUsuarioService
    {
        private readonly VillaAvocadoDbContext _context;
        public UsuarioService(VillaAvocadoDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario?> Login(string correo, string password)
        {
            var hashedPassword = HashPassword(password);
            return await _context.Usuarios
                .Where(u => u.Correo == correo && u.Password == hashedPassword && u.Activo)
                .FirstOrDefaultAsync();
        }
        public async Task<Usuario> Register(string nombre, string correo, string password)
        {
            // Verificar si el correo ya existe
            if (await CorreoExiste(correo))
            {
                throw new InvalidOperationException("El correo ya está registrado");
            }

            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                Password = HashPassword(password),
                Rol = RolUsuario.Usuario,
                Activo = true,
                CreatedAt = DateTime.Now,
                UserCreateId = null, 
                DeletedAt = null,     
                LastModification = null 
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            return await _context.Usuarios
                .Where(u => u.Activo)
                .Include(u => u.Diplomados)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }
        public async Task<Usuario?> GetUsuarioById(int id)
        {
            return await _context.Usuarios
                .Where(u => u.Id == id && u.Activo)
                .FirstOrDefaultAsync();
        }
        public async Task<Usuario?> GetUsuarioConDiplomados(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Diplomados)
                    .ThenInclude(cd => cd.Diplomado)
                        .ThenInclude(d => d.Materias.OrderBy(m => m.Orden))
                .Where(u => u.Id == id && u.Activo)
                .FirstOrDefaultAsync();
        }
        public async Task<Usuario> UpdateUsuario(int id, string? nombre, string? correo, RolUsuario? rol, bool? activo)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            if (!string.IsNullOrWhiteSpace(nombre))
                usuario.Nombre = nombre;

            if (!string.IsNullOrWhiteSpace(correo))
            {
                if (await CorreoExiste(correo, id))
                {
                    throw new InvalidOperationException("El correo ya está en uso");
                }
                usuario.Correo = correo;
            }
            if (rol.HasValue)
                usuario.Rol = rol.Value;
            if (activo.HasValue)
                usuario.Activo = activo.Value;
            usuario.LastModification = DateTime.Now;
            await _context.SaveChangesAsync();
            return usuario;
        }
        public async Task<bool> DeleteUsuario(int id, int? userDeleteId)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;
            usuario.Activo = false;
            usuario.DeletedAt = DateTime.Now;
            usuario.UserDeleteId = userDeleteId;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CorreoExiste(string correo, int? exceptoId = null)
        {
            var query = _context.Usuarios.Where(u => u.Correo == correo);
            if (exceptoId.HasValue)
                query = query.Where(u => u.Id != exceptoId.Value);
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<object>> GetDiplomadosDeUsuario(int usuarioId)
        {
            return await _context.CargaDiplomados
                .Include(cd => cd.Diplomado)
                    .ThenInclude(d => d.Materias.OrderBy(m => m.Orden))
                .Where(cd => cd.UsuarioId == usuarioId)
                .Select(cd => new
                {
                    cd.Id,
                    cd.DiplomadoId,
                    Diplomado = new
                    {
                        cd.Diplomado.Id,
                        cd.Diplomado.Nombre,
                        cd.Diplomado.Descripcion,
                        cd.Diplomado.FechaInicio,
                        cd.Diplomado.FechaFin,
                        Materias = cd.Diplomado.Materias.Select(m => new
                        {
                            m.Id,
                            m.Nombre,
                            m.Descripcion,
                            m.Orden,
                            m.DuracionHoras
                        })
                    },
                    cd.FechaInscripcion,
                    cd.Estado,
                    cd.Calificacion
                })
                .ToListAsync();
        }

        public string HashPassword(string password)
        {
            // En producción, usa BCrypt, PBKDF2 o Argon2
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

}
