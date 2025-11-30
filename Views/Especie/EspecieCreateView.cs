using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Views.Especie
{
    public class EspecieCreateView
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string Nome { get; set; } = string.Empty;
    }
}
