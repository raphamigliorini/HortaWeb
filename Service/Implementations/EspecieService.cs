using Esg.Horta.Entities;
using FiapWebAluno.Data.Contexts;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Especie;
using FiapWebAluno.Views.Common;
using Microsoft.EntityFrameworkCore;

namespace FiapWebAluno.Service.Implementations
{
    /// <summary>
    /// Serviço de domínio para ESPECIE.
    /// Responsável por:
    /// - Paginação (ListarAsync)
    /// - Criação com validação de ID único (CriarAsync)
    /// Esse service é usado pelos controllers para manter o padrão MVVM.
    /// </summary>
    public class EspecieService : IEspecieService
    {
        private readonly DatabaseContext _context;

        public EspecieService(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista espécies com paginação, retornando um PagedResultView
        /// contendo:
        /// - Items (EspecieListItemView)
        /// - Page / PageSize
        /// - TotalItems
        /// Esse método é usado pela pessoa 2 para atender o requisito de paginação.
        /// </summary>
        public async Task<PagedResultView<EspecieListItemView>> ListarAsync(int page, int pageSize)
        {
            var query = _context.Especies.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EspecieListItemView
                {
                    Id = e.Id,
                    // e.Nome pode ser nulo em dados legados, então garantimos string vazia
                    Nome = e.Nome ?? string.Empty
                })
                .ToListAsync();

            return new PagedResultView<EspecieListItemView>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        /// <summary>
        /// Cria uma nova espécie garantindo:
        /// - ID não duplicado (regra de negócio)
        /// - Nome preenchido (fallback para string.Empty se vier nulo)
        /// </summary>
        public async Task<EspecieListItemView> CriarAsync(EspecieCreateView model)
        {
            // Regra de negócio: ID não pode repetir, pois o Oracle não gera automaticamente
            if (await _context.Especies.AnyAsync(x => x.Id == model.Id))
                throw new Exception("ID já existe.");

            var entity = new Especie
            {
                Id = model.Id,
                Nome = model.Nome ?? string.Empty
            };

            _context.Especies.Add(entity);
            await _context.SaveChangesAsync();

            return new EspecieListItemView
            {
                Id = entity.Id,
                Nome = entity.Nome
            };
        }
    }
}
