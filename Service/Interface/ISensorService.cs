using FiapWebAluno.Views.Common;
using FiapWebAluno.Views.SensorUmidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Interface
{
    public interface ISensorService
    {
        Task<PagedResultView<SensorUmidadeListItemView>> ListarAsync (int page, int pageSize);
    }
}
