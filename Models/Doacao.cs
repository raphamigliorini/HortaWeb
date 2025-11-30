using System.ComponentModel.DataAnnotations;


using System.ComponentModel.DataAnnotations.Schema;


namespace FiapWebAluno.Models
{
    [Table("TB_DOACAO")]
    public class Doacao
    {
        [Key]
        [Column("ID_DOACAO")]
        public long IdDoacao { get; set; }

        [Column("ID_COLHEITA")]
        public long IdColheita { get; set; }

        [Column("ENTIDADE")]
        public string Entidade { get; set; } = string.Empty;

        [Column("QUANTIDADE_KG", TypeName = "NUMBER(9,2)")]
        public decimal QuantidadeKg { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [ForeignKey(nameof(IdColheita))]
        public Colheita? Colheita { get; set; }
    }
}
