using SQLite;

namespace PsicoSync.Model
{
    public class objCita
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Tipo { get; set; }
        public string Modalidad { get; set; }
        [Ignore]
        public objTipoCita TipoCita { get; set; }
        public int TipoCitaID { get; set; }
        public DateTime Fecha { get; set; }
        [Ignore]
        public string FechaString { get; set; }
        [Ignore]
        public objCliente Cliente { get; set; }
        public int ClienteID { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        // Agendada, Finalizada
    }
}
