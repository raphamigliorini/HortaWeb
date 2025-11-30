using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Views.Canteiro
{
    public class CanteiroCreateView
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public long EspecieId { get; set; }

        public decimal? AreaM2 {  get; set; }
        public decimal? MetaDoacaoKg { get; set; }
    }
}
