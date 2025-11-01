namespace Diplomados_VillaVocado.Models
{
    public enum RolUsuario
    {
        Usuario = 1,
        Administrador = 2
    }
    public class Usuario: AuditableEntity
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public required string Password { get; set; }
        public RolUsuario Rol { get; set; } = RolUsuario.Usuario;
        public bool Activo { get; set; } = true;
        // Relación: Un usuario puede tener múltiples diplomados
        public required ICollection<CargaDiplomado> Diplomados { get; set; }
    }
}
