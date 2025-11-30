using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FiapWebAluno.Models; // SensorUmidade, Irrigacao, Colheita

namespace Esg.Horta.Entities
{
    /// <summary>
    /// Entidade de canteiro da horta ESG.
    /// Mapeia a tabela CANTEIRO no Oracle.
    /// </summary>
    [Table("CANTEIRO")]
    public class Canteiro
    {
        /// <summary>
        /// Chave primária do canteiro.
        /// Também não é gerada automaticamente pelo EF.
        /// </summary>
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        /// Nome do canteiro.
        /// Deixamos como string? (nullable) porque há registros na base
        /// em que o campo NOME está NULL, e isso gerava o erro:
        /// "A coluna contém dados NULL" ao projetar para string não anulável.
        /// </summary>
        [Column("NOME", TypeName = "VARCHAR2(255)")]
        public string? Nome { get; set; }

        /// <summary>
        /// Chave estrangeira para ESPECIE.
        /// </summary>
        [Column("ESPECIE_ID")]
        public long EspecieId { get; set; }

        /// <summary>
        /// Área em metros quadrados.
        /// </summary>
        [Column("AREA_M2", TypeName = "NUMBER(7,2)")]
        public decimal? AreaM2 { get; set; }

        /// <summary>
        /// Meta de doação em kg.
        /// </summary>
        [Column("META_DOACAO_KG", TypeName = "NUMBER(9,2)")]
        public decimal? MetaDoacaoKg { get; set; }

        // ==========================
        // Navegações (relacionamentos)
        // ==========================

        /// <summary>
        /// Relação 1:N com sensores de umidade ligados ao canteiro.
        /// </summary>
        public ICollection<SensorUmidade> SensoresUmidade { get; set; } = new List<SensorUmidade>();

        /// <summary>
        /// Relação 1:N com registros de irrigação do canteiro.
        /// </summary>
        public ICollection<Irrigacao> Irrigacoes { get; set; } = new List<Irrigacao>();

        /// <summary>
        /// Relação 1:N com colheitas realizadas no canteiro.
        /// </summary>
        public ICollection<Colheita> Colheitas { get; set; } = new List<Colheita>();
    }
}
