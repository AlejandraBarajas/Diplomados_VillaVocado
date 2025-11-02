namespace Diplomados_VillaVocado.Models
{
    public enum EstadoDiplomado
    {
        Inscrito = 1,
        EnCurso = 2,
        Completado = 3,
        Cancelado = 4
    }
    public class CargaDiplomado: AuditableEntity
    {

        public int Id { get; set; }
        // Relación con Usuario
        public int UsuarioId { get; set; }  // Llave foránea
        public Usuario Usuario { get; set; }  // Propiedad de navegación
        // Relación con el diplomado
        public int DiplomadoId { get; set; }  // Llave foránea
        public Diplomado Diplomado { get; set; }  // Propiedad de navegación
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
        public EstadoDiplomado Estado { get; set; } = EstadoDiplomado.Inscrito;
        public DateTime? FechaCompletado { get; set; }
        public decimal? Calificacion { get; set; }
    }
}
