namespace Diplomados_VillaVocado.Models
{
    public class Usuario: AuditableEntity
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public required ICollection<CargaDiplomado> Diplomados { get; set; }
    }
}
