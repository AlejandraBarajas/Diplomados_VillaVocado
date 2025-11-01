namespace Diplomados_VillaVocado.Models
{
    public class Materia : AuditableEntity
    {
        public int Id { get; set; }
        public  required string Nombre { get; set; }
        public required ICollection<CargaMateria> Materias { get; set; }
    }
}
