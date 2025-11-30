using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esg.Horta.Entities
{
    /// <summary>
    /// Entidade de espécie cultivada na horta ESG.
    /// Mapeia a tabela ESPECIE do Oracle.
    /// </summary>
    [Table("ESPECIE")]
    public class Especie
    {
        /// <summary>
        /// Chave primária (ID).
        /// No Oracle é preenchido via sequence/configuração externa,
        /// por isso usamos DatabaseGeneratedOption.None.
        /// </summary>
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        /// Nome da espécie.
        /// Importante: deixamos como string? (nullable) porque a tabela
        /// pode conter registros com NOME = NULL, o que causava
        /// InvalidCastException quando o EF lia como string não anulável.
        /// </summary>
        [StringLength(60)]
        [Column("NOME", TypeName = "VARCHAR2(60)")]
        public string? Nome { get; set; }
    }
}
