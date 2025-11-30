using FiapWebAluno.Data.Contexts;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Common;
using FiapWebAluno.Views.SensorUmidade;
using Microsoft.EntityFrameworkCore;

namespace FiapWebAluno.Service.Implementations
{
    public class SensorService : ISensorService
    {
        private readonly DatabaseContext _context;

        public SensorService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PagedResultView<SensorUmidadeListItemView>> ListarAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.SensoresUmidade
                                .AsNoTracking()
                                .OrderBy(s => s.IdSensor);

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SensorUmidadeListItemView
                {
                    IdSensor = s.IdSensor,
                    IdCanteiro = s.IdCanteiro,
                    DataHora = s.DataHora,
                    PercentualHumidade = s.PercentualUmidade
                })
                .ToListAsync();

            return new PagedResultView<SensorUmidadeListItemView>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }
    }
}
