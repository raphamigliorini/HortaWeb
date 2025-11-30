using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Views.Irrigacao
{
    public class IrrigacaoCreateView
    {
        [Required]
        public long IdIrrigacao { get; set; }

        [Required]
        public long IdCanteiro { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Litros deve ser maior que zero.")]
        public decimal Litros { get; set; }

        [MaxLength(20)]
        public string Origem { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Observacao { get; set; }
    }
}
