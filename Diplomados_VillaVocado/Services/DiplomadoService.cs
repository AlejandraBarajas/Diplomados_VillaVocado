using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplomados_VillaVocado.Services
{
    public class DiplomadoService : IDiplomadoService
    {
        private readonly VillaAvocadoDbContext _context;

        public DiplomadoService(VillaAvocadoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Diplomado>> GetAllDiplomados()
        {
            return await _context.Diplomados
                .Where(d => d.Activo)
                .Include(d => d.Materias)
                .Include(d => d.Diplomados)
                .OrderBy(d => d.Nombre)
                .ToListAsync();
        }

        public async Task<Diplomado?> GetDiplomadoById(int id)
        {
            return await _context.Diplomados
                .Where(d => d.Id == id && d.Activo)
                .FirstOrDefaultAsync();
        }

        public async Task<Diplomado?> GetDiplomadoConDetalles(int id)
        {
            return await _context.Diplomados
                .Where(d => d.Id == id && d.Activo)
                .Include(d => d.Materias.Where(m => m.Activo))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Diplomado>> GetDiplomadosDisponibles(int? usuarioId = null)
        {
            var query = _context.Diplomados
                .Where(d => d.Activo)
                .Include(d => d.Materias.Where(m => m.Activo))
                .OrderBy(d => d.Nombre);

            // Si hay usuarioId, excluir los diplomados ya inscritos
            if (usuarioId.HasValue)
            {
                var diplomadosInscritos = await _context.CargaDiplomados
                    .Where(cd => cd.UsuarioId == usuarioId.Value)
                    .Select(cd => cd.DiplomadoId)
                    .ToListAsync();

                return await query
                    .Where(d => !diplomadosInscritos.Contains(d.Id))
                    .ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<Diplomado> CreateDiplomado(string nombre, string? descripcion, DateTime fechaInicio, DateTime fechaFin, int? userCreateId)
        {
            var diplomado = new Diplomado
            {
                Nombre = nombre,
                Descripcion = descripcion,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Activo = true,
                CreatedAt = DateTime.Now,
                UserCreateId = userCreateId
            };

            _context.Diplomados.Add(diplomado);
            await _context.SaveChangesAsync();

            return diplomado;
        }

        public async Task<Diplomado> UpdateDiplomado(int id, string? nombre, string? descripcion, DateTime? fechaInicio, DateTime? fechaFin, bool? activo)
        {
            var diplomado = await _context.Diplomados.FindAsync(id);
            if (diplomado == null)
            {
                throw new KeyNotFoundException("Diplomado no encontrado");
            }

            if (!string.IsNullOrWhiteSpace(nombre))
                diplomado.Nombre = nombre;

            if (descripcion != null)
                diplomado.Descripcion = descripcion;

            if (fechaInicio.HasValue)
                diplomado.FechaInicio = fechaInicio.Value;

            if (fechaFin.HasValue)
                diplomado.FechaFin = fechaFin.Value;

            if (activo.HasValue)
                diplomado.Activo = activo.Value;

            diplomado.LastModification = DateTime.Now;
            await _context.SaveChangesAsync();

            return diplomado;
        }

        public async Task<bool> DeleteDiplomado(int id, int? userDeleteId)
        {
            var diplomado = await _context.Diplomados.FindAsync(id);
            if (diplomado == null)
                return false;

            diplomado.Activo = false;
            diplomado.DeletedAt = DateTime.Now;
            diplomado.UserDeleteId = userDeleteId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CargaDiplomado> InscribirUsuario(int usuarioId, int diplomadoId)
        {
            // Verificar si el usuario ya está inscrito
            if (await UsuarioYaInscrito(usuarioId, diplomadoId))
            {
                throw new InvalidOperationException("El usuario ya está inscrito en este diplomado");
            }

            var cargaDiplomado = new CargaDiplomado
            {
                UsuarioId = usuarioId,
                DiplomadoId = diplomadoId,
                FechaInscripcion = DateTime.Now,
                Estado = EstadoDiplomado.Inscrito,
                CreatedAt = DateTime.Now,
                UserCreateId = usuarioId
            };

            _context.CargaDiplomados.Add(cargaDiplomado);
            await _context.SaveChangesAsync();

            return cargaDiplomado;
        }

        public async Task<bool> UsuarioYaInscrito(int usuarioId, int diplomadoId)
        {
            return await _context.CargaDiplomados
                .AnyAsync(cd => cd.UsuarioId == usuarioId && cd.DiplomadoId == diplomadoId);
        }

        public async Task<bool> CancelarInscripcion(int cargaDiplomadoId, int usuarioId)
        {
            var carga = await _context.CargaDiplomados
                .Where(cd => cd.Id == cargaDiplomadoId && cd.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (carga == null)
                return false;

            carga.Estado = EstadoDiplomado.Cancelado;
            carga.LastModification = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CargaDiplomado>> GetCargasDiplomados(int? usuarioId = null)
        {
            IQueryable<CargaDiplomado> query = _context.CargaDiplomados;

            if (usuarioId.HasValue)
            {
                query = query.Where(cd => cd.UsuarioId == usuarioId.Value);
            }

            return await query
                .Include(cd => cd.Usuario)
                .Include(cd => cd.Diplomado)
                    .ThenInclude(d => d.Materias)
                .OrderByDescending(cd => cd.FechaInscripcion)
                .ToListAsync();
        }
    }
}