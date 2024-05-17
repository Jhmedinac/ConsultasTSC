using System.ComponentModel.DataAnnotations;

namespace ConsultasTSC.Models
{
    public class Cerveza
    {
        [Key]
        public int BeerID { get; set; }
        public string Nombre { get; set; }
        public string Pais_origen { get; set; }
        public decimal Porcentaje_alcohol { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_creacion { get; set; }
    }
}
