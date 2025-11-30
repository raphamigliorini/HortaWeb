using FiapWebAluno.Views.Common;
using FiapWebAluno.Views.Irrigacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Interface
{
    public interface IIrrigacaoService
    {
        Task<PagedResultView<IrrigacaoListItemView>> ListarAsync(int page, int pageSize);
        Task<IrrigacaoListItemView> CriarAsync(IrrigacaoCreateView model);
    }
}
