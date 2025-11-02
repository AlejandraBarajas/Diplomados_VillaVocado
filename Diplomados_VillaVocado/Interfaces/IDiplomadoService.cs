using Diplomados_VillaVocado.Models;

namespace Diplomados_VillaVocado.Interfaces
{
    public interface IDiplomadoService
    {
        Task<IEnumerable<Diplomado>> GetAllDiplomados();
        Task<Diplomado?> GetDiplomadoById(int id);
        Task<Diplomado?> GetDiplomadoConDetalles(int id);
        Task<IEnumerable<Diplomado>> GetDiplomadosDisponibles(int? usuarioId = null);
        Task<Diplomado> CreateDiplomado(string nombre, string? descripcion, DateTime fechaInicio, DateTime fechaFin, int? userCreateId);
        Task<Diplomado> UpdateDiplomado(int id, string? nombre, string? descripcion, DateTime? fechaInicio, DateTime? fechaFin, bool? activo);
        Task<bool> DeleteDiplomado(int id, int? userDeleteId);

        // Métodos para inscripciones
        Task<CargaDiplomado> InscribirUsuario(int usuarioId, int diplomadoId);
        Task<bool> UsuarioYaInscrito(int usuarioId, int diplomadoId);
        Task<bool> CancelarInscripcion(int cargaDiplomadoId, int usuarioId);
        Task<IEnumerable<CargaDiplomado>> GetCargasDiplomados(int? usuarioId = null);
    }
}
