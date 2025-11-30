using Esg.Horta.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiapWebAluno.Models
{
    [Table("TB_SENSOR_UMIDADE")]
    public class SensorUmidade
    {
        [Key]
        [Column("ID_SENSOR")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IdSensor { get; set; }

        [Required]
        [Column("ID_CANTEIRO")]
        public long IdCanteiro { get; set; }

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "PERCENTUAL_UMIDADE deve estar entre 0 e 100")]
        [Column("PERCENTUAL_UMIDADE", TypeName = "NUMBER(5,2)")]
        public decimal PercentualUmidade { get; set; }

        // FK Navigation
        [ForeignKey(nameof(IdCanteiro))]
        public Canteiro? Canteiro { get; set; }

    }
}
