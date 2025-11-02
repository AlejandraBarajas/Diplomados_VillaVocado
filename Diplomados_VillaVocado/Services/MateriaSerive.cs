using Diplomados_VillaVocado.Interfaces;
using Diplomados_VillaVocado.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplomados_VillaVocado.Services
{
    public class MateriaService : IMateriaService
    {
        private readonly VillaAvocadoDbContext _context;

        public MateriaService(VillaAvocadoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Materia>> GetAllMaterias()
        {
            return await _context.Materias
                .Where(m => m.Activo)
                .Include(m => m.Diplomado)
                .OrderBy(m => m.Diplomado.Nombre)
                .ThenBy(m => m.Orden)
                .ToListAsync();
        }

        public async Task<IEnumerable<Materia>> GetMateriasPorDiplomado(int diplomadoId)
        {
            return await _context.Materias
                .Where(m => m.DiplomadoId == diplomadoId && m.Activo)
                .OrderBy(m => m.Orden)
                .ToListAsync();
        }

        public async Task<Materia?> GetMateriaById(int id)
        {
            return await _context.Materias
                .Where(m => m.Id == id && m.Activo)
                .Include(m => m.Diplomado)
                .FirstOrDefaultAsync();
        }

        public async Task<Materia> CreateMateria(string nombre, string? descripcion, int orden, int duracionHoras, int diplomadoId, int? userCreateId)
        {
            // Verificar si ya existe una materia con ese orden en el diplomado
            var materiaExistente = await _context.Materias
                .AnyAsync(m => m.DiplomadoId == diplomadoId && m.Orden == orden);

            if (materiaExistente)
            {
                throw new InvalidOperationException("Ya existe una materia con ese orden en este diplomado");
            }

            var materia = new Materia
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Orden = orden,
                DuracionHoras = duracionHoras,
                DiplomadoId = diplomadoId,
                Activo = true,
                CreatedAt = DateTime.Now,
                UserCreateId = userCreateId
            };

            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();

            return materia;
        }

        public async Task<Materia> UpdateMateria(int id, string? nombre, string? descripcion, int? orden, int? duracionHoras, bool? activo)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                throw new KeyNotFoundException("Materia no encontrada");
            }

            if (!string.IsNullOrWhiteSpace(nombre))
                materia.Nombre = nombre;

            if (descripcion != null)
                materia.Descripcion = descripcion;

            if (orden.HasValue)
            {
                // Verificar si el nuevo orden ya existe en el mismo diplomado
                var ordenExistente = await _context.Materias
                    .AnyAsync(m => m.DiplomadoId == materia.DiplomadoId && m.Orden == orden.Value && m.Id != id);

                if (ordenExistente)
                {
                    throw new InvalidOperationException("Ya existe una materia con ese orden en este diplomado");
                }

                materia.Orden = orden.Value;
            }

            if (duracionHoras.HasValue)
                materia.DuracionHoras = duracionHoras.Value;

            if (activo.HasValue)
                materia.Activo = activo.Value;

            materia.LastModification = DateTime.Now;
            await _context.SaveChangesAsync();

            return materia;
        }

        public async Task<bool> DeleteMateria(int id, int? userDeleteId)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
                return false;

            materia.Activo = false;
            materia.DeletedAt = DateTime.Now;
            materia.UserDeleteId = userDeleteId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
