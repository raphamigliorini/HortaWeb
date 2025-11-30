using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Views.SensorUmidade
{
    public class SensorUmidadeListItemView
    {
        public long IdSensor { get; set; }
        public long IdCanteiro { get; set; }
        public DateTime DataHora { get; set; }
        public decimal PercentualHumidade { get; set; }
    }
}
