using Esg.Horta.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FiapWebAluno.Models
{
    [Table("TB_COLHEITA")]
    public class Colheita
    {
        [Key]
        [Column("ID_COLHEITA")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IdColheita { get; set; }

        [Required]
        [Column("ID_CANTEIRO")]
        public long IdCanteiro { get; set; }

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Range(0.0000001, double.MaxValue, ErrorMessage = "QUANTIDADE_KG deve ser > 0")]
        [Column("QUANTIDADE_KG", TypeName = "NUMBER(9,2)")]
        public decimal QuantidadeKg { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("^(CONSUMO|DOACAO|MISTO)$", ErrorMessage = "DESTINO deve ser CONSUMO, DOACAO ou MISTO")]
        [Column("DESTINO", TypeName = "VARCHAR2(20)")]
        public string Destino { get; set; } = string.Empty;

        [StringLength(20)]
        [RegularExpression("^(CONSUMO|DOACAO|MISTO)$", ErrorMessage = "DESTINO_SUGERIDO deve ser CONSUMO, DOACAO ou MISTO")]
        [Column("DESTINO_SUGERIDO", TypeName = "VARCHAR2(20)")]
        public string? DestinoSugerido { get; set; }

        // FK Navigation
        [ForeignKey(nameof(IdCanteiro))]
        public Canteiro? Canteiro { get; set; }

        public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();


    }
}
