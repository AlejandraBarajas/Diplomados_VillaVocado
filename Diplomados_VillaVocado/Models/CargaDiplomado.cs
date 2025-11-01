namespace Diplomados_VillaVocado.Models
{
    public class CargaDiplomado: AuditableEntity
    {
        public int Id { get; set; }
        // Relación con Usuario
        public int UsuarioId { get; set; }  // Llave foránea
        public required Usuario Usuario { get; set; }  // Propiedad de navegación
        // Relación con el diplomado
        public int DiplomadoId { get; set; }  // Llave foránea
        public required Diplomado Diplomado { get; set; }  // Propiedad de navegación
    }
}
