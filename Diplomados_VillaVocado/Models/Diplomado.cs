namespace Diplomados_VillaVocado.Models
{
    public class Diplomado: AuditableEntity
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public required ICollection<CargaDiplomado> Diplomados { get; set; }
        public required ICollection<CargaMateria> Materias { get; set; }
    }
}
