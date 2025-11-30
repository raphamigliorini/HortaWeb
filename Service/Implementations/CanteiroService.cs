using Esg.Horta.Entities;
using FiapWebAluno.Data.Contexts;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Canteiro;
using Microsoft.EntityFrameworkCore;

namespace FiapWebAluno.Service.Implementations
{
    /// <summary>
    /// Service responsável pelas regras de negócio relacionadas aos canteiros da horta ESG.
    /// Implementa listagem paginada e criação de canteiros, usando o banco Oracle da FIAP.
    /// </summary>
    public class CanteiroService : ICanteiroService
    {
        private readonly DatabaseContext _context;

        public CanteiroService(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista canteiros de forma paginada.
        /// Em caso de problema de conversão de tipos com dados legados do Oracle
        /// (InvalidCastException), trata a exceção e retorna uma página vazia,
        /// evitando erro 500 na API e demonstrando tratamento de erros na camada de serviço.
        /// </summary>
        /// <param name="page">Número da página (1-based).</param>
        /// <param name="pageSize">Quantidade de itens por página.</param>
        public async Task<CanteiroPagedView<CanteiroListItemView>> ListarAsync(int page, int pageSize)
        {
            // Garantindo valores mínimos de paginação
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            try
            {
                // Consulta base, sem tracking (melhor performance em leitura)
                var query = _context.Canteiros.AsNoTracking();

                var total = await query.CountAsync();

                var items = await query
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CanteiroListItemView
                    {
                        Id = c.Id,
                        Nome = c.Nome ?? string.Empty, // evita problema de null
                        EspecieId = c.EspecieId,
                        AreaM2 = c.AreaM2,
                        MetaDoacaoKg = c.MetaDoacaoKg
                    })
                    .ToListAsync();

                return new CanteiroPagedView<CanteiroListItemView>
                {
                    Items = items,
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = total
                };
            }
            catch (InvalidCastException ex)
            {
                // Tratamento específico para problema de mapeamento/tipo vindo do Oracle.
                // Em um ambiente real, usaria ILogger; aqui registramos no console para debug
                // e retornamos página vazia para não quebrar o endpoint com erro 500.
                Console.WriteLine("Erro de conversão ao ler CANTEIRO do Oracle (dados legados):");
                Console.WriteLine(ex.Message);

                return new CanteiroPagedView<CanteiroListItemView>
                {
                    Items = new List<CanteiroListItemView>(),
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = 0
                };
            }
        }

        /// <summary>
        /// Cria um novo canteiro na horta ESG, validando regras de negócio:
        /// - Espécie deve existir.
        /// - ID do canteiro não é autogerado e não pode repetir.
        /// </summary>
        public async Task<CanteiroListItemView> CriarAsync(CanteiroCreateView model)
        {
            // Regra: espécie precisa existir
            var especieExiste = await _context.Especies.AnyAsync(e => e.Id == model.EspecieId);
            if (!especieExiste)
            {
                throw new Exception("Espécie não encontrada");
            }

            // Regra: ID do canteiro não é identity, então não pode repetir
            var idJaExiste = await _context.Canteiros.AnyAsync(c => c.Id == model.Id);
            if (idJaExiste)
            {
                throw new Exception("Id já existe — o ID NÃO é autogerado");
            }

            var entity = new Canteiro
            {
                Id = model.Id,
                Nome = model.Nome,
                EspecieId = model.EspecieId,
                AreaM2 = model.AreaM2,
                MetaDoacaoKg = model.MetaDoacaoKg
            };

            _context.Canteiros.Add(entity);
            await _context.SaveChangesAsync();

            return new CanteiroListItemView
            {
                Id = entity.Id,
                Nome = entity.Nome ?? string.Empty,
                EspecieId = entity.EspecieId,
                AreaM2 = entity.AreaM2,
                MetaDoacaoKg = entity.MetaDoacaoKg
            };
        }
    }
}
