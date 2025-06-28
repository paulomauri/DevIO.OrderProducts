using DevIO.OrderProducts.Domain.Entities;
using DevIO.OrderProducts.Domain.Interfaces;
using DevIO.OrderProducts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace DevIO.OrderProducts.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id)
    {
        return await _context.Pedido
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        return await _context.Pedido
            .Include(p => p.Itens)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AdicionarAsync(Pedido pedido)
    {
        await _context.Pedido.AddAsync(pedido);
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        _context.Pedido.Update(pedido);
        await Task.CompletedTask;
    }

    public async Task RemoverAsync(Pedido pedido)
    {
        _context.Pedido.Remove(pedido);
        await Task.CompletedTask;
    }
}
