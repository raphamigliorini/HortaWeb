using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Views.Canteiro
{
    public class CanteiroListItemView
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public long EspecieId { get; set; }
        public decimal? AreaM2 { get; set; }
        public decimal? MetaDoacaoKg { get; set; }
    }
}
