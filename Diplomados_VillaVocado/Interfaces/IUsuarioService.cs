using Diplomados_VillaVocado.Models;

namespace Diplomados_VillaVocado.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> Login(string correo, string password);
        //Task<Usuario?> Logout(); 
        Task<Usuario> Register(string nombre, string correo, string password);
        Task<IEnumerable<Usuario>> GetAllUsuarios();
        Task<Usuario?> GetUsuarioById(int id);
        Task<Usuario?> GetUsuarioConDiplomados(int id);
        Task<Usuario> UpdateUsuario(int id, string? nombre, string? correo, RolUsuario? rol, bool? activo);
        Task<bool> DeleteUsuario(int id, int? userDeleteId);
        Task<bool> CorreoExiste(string correo, int? exceptoId = null);
        Task<IEnumerable<object>> GetDiplomadosDeUsuario(int usuarioId);
        string HashPassword(string password);
    }
}
