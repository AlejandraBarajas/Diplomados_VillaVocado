namespace Diplomados_VillaVocado.Models
{
    public class Materia : AuditableEntity
    {
        public int Id { get; set; }
        public  required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public int DuracionHoras { get; set; }
        public bool Activo { get; set; } = true;
        public int DiplomadoId { get; set; }
        public Diplomado? Diplomado { get; set; }
    }
}
