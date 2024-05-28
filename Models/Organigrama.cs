using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultasTSC.Models
{
    public class Organigrama
    {
        [Key]
        [Column("codigo_organigrama")]
        public int CodigoOrganigrama { get; set; }

        [Required]
        [Column("descripcion_organigrama")]
        [MaxLength(250)]
        public string DescripcionOrganigrama { get; set; }

        [Column("codigo_organigrama_padre")]
        public int? CodigoOrganigramaPadre { get; set; }

        [Required]
        [Column("estado")]
        public bool Estado { get; set; }

        [Required]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_modificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Required]
        [Column("usuario_creacion")]
        public int UsuarioCreacion { get; set; }

        [Column("usuario_modificacion")]
        public int? UsuarioModificacion { get; set; }
    }
}
