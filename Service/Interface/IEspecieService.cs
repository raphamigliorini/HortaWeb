using FiapWebAluno.Views.Canteiro;
using FiapWebAluno.Views.Common;
using FiapWebAluno.Views.Especie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Interface
{
    public interface IEspecieService
    {
        Task<PagedResultView<EspecieListItemView>> ListarAsync(int page, int pageSize);
        Task<EspecieListItemView> CriarAsync(EspecieCreateView model);
    }
}
