using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoSync.Model
{
    public class objCliente
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Ocupacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [Ignore]
        public string FechaNacimientoString { get; set; }
        public string AntecedentesMedicos { get; set; }
        public string AntecedentesFamiliares { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
    }
}
