namespace RazorPagesDB.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string nombreTarea { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public String estado { get; set; }
        public int IdUsuario { get; set; }
    }
}
