using Diplomados_VillaVocado.Models;

namespace Diplomados_VillaVocado.Interfaces
{
    public interface IMateriaService
    {
        Task<IEnumerable<Materia>> GetAllMaterias();
        Task<IEnumerable<Materia>> GetMateriasPorDiplomado(int diplomadoId);
        Task<Materia?> GetMateriaById(int id);
        Task<Materia> CreateMateria(string nombre, string? descripcion, int orden, int duracionHoras, int diplomadoId, int? userCreateId);
        Task<Materia> UpdateMateria(int id, string? nombre, string? descripcion, int? orden, int? duracionHoras, bool? activo);
        Task<bool> DeleteMateria(int id, int? userDeleteId);
    }
}
