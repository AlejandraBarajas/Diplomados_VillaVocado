namespace Diplomados_VillaVocado.Models
{
    public class Diplomado: AuditableEntity
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; } = true;
        // Relación: Usuarios inscritos en este diplomado
        public required ICollection<CargaDiplomado> Diplomados { get; set; }
        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
