using FiapWebAluno.Views.Canteiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Interface
{
    public interface ICanteiroService
    {
        Task<CanteiroPagedView<CanteiroListItemView>> ListarAsync(int page, int pageSize);
        Task<CanteiroListItemView> CriarAsync(CanteiroCreateView model);
    }
}
