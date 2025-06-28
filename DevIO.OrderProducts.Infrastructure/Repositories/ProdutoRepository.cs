using DevIO.OrderProducts.Domain.Entities;
using DevIO.OrderProducts.Domain.Interfaces;
using DevIO.OrderProducts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DevIO.OrderProducts.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        public readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produto.AddAsync(produto);
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produto.Update(produto);
            await Task.CompletedTask;
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _context.Produto
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produto
                .AsNoTracking()
                .ToListAsync(); 
        }

        public Task RemoverAsync(Produto produto)
        {
            _context.Remove(produto);
            return Task.CompletedTask;
        }
    }
}
