using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Views.Irrigacao
{
    public class IrrigacaoListItemView
    {
        public long IdIrrigacao { get; set; }
        public long IdCanteiro { get; set; }
        public decimal Litros { get; set; }
        public DateTime DataHora { get; set; }
        public string Origem { get; set; } = string.Empty;
    }
}
