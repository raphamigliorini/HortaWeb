using FiapWebAluno.Data.Contexts;
using FiapWebAluno.Models;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Common;
using FiapWebAluno.Views.Irrigacao;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Implementations
{
    public class IrrigacaoService : IIrrigacaoService
    {
        private readonly DatabaseContext _context;

        public IrrigacaoService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PagedResultView<IrrigacaoListItemView>> ListarAsync(int page, int pageSize)
        {
            var query = _context.Irrigacoes.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(i => i.IdIrrigacao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new IrrigacaoListItemView
                {
                    IdIrrigacao = i.IdIrrigacao,
                    IdCanteiro = i.IdCanteiro,
                    Litros = i.Litros,
                    DataHora = i.DataHora,
                    Origem = i.Origem
                })
                .ToListAsync();

            return new PagedResultView<IrrigacaoListItemView>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        public async Task<IrrigacaoListItemView> CriarAsync(IrrigacaoCreateView model)
        {
            if (!await _context.Canteiros.AnyAsync(c => c.Id == model.IdCanteiro))
                throw new Exception("Canteiro não existe.");

            if (await _context.Irrigacoes.AnyAsync(i => i.IdIrrigacao == model.IdIrrigacao))
                throw new Exception("ID de irrigação já existe.");

            var entity = new Irrigacao
            {
                IdIrrigacao = model.IdIrrigacao,
                IdCanteiro = model.IdCanteiro,
                DataHora = model.DataHora,
                Litros = model.Litros,
                Origem = model.Origem,
                Observacao = model.Observacao
            };

            _context.Irrigacoes.Add(entity);
            await _context.SaveChangesAsync();

            return new IrrigacaoListItemView
            {
                IdIrrigacao = entity.IdIrrigacao,
                IdCanteiro = entity.IdCanteiro,
                Litros = entity.Litros,
                DataHora = entity.DataHora,
                Origem = entity.Origem
            };
        }

        Task<PagedResultView<IrrigacaoListItemView>> IIrrigacaoService.ListarAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        //public Task<IrrigacaoListItemView> CriarAsync(IrrigacaoCreateView model)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
