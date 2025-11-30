using Esg.Horta.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiapWebAluno.Models
{
    [Table("TB_IRRIGACAO")]
    public class Irrigacao
    {
        [Key]
        [Column("ID_IRRIGACAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IdIrrigacao { get; set; }

        [Required]
        [Column("ID_CANTEIRO")]
        public long IdCanteiro { get; set; }

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Range(0.0000001, double.MaxValue, ErrorMessage = "LITROS deve ser > 0")]
        [Column("LITROS", TypeName = "NUMBER(9,2)")]
        public decimal Litros { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("^(AUTOMATICA|MANUAL)$", ErrorMessage = "ORIGEM deve ser AUTOMATICA ou MANUAL")]
        [Column("ORIGEM", TypeName = "VARCHAR2(20)")]
        public string Origem { get; set; } = string.Empty;

        [StringLength(200)]
        [Column("OBSERVACAO", TypeName = "VARCHAR2(200)")]
        public string? Observacao { get; set; }

        // FK Navigation
        [ForeignKey(nameof(IdCanteiro))]
        public Canteiro? Canteiro { get; set; }
    }
}
