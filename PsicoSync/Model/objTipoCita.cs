using SQLite;

namespace PsicoSync.Model
{
    public class objTipoCita
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int DuracionMinutos { get; set; }
        public decimal Precio { get; set; }
    }
}
