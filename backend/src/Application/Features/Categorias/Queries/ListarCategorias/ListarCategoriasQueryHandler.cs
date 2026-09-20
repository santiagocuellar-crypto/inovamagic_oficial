using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Application.Features.Categorias.Queries.ListarCategorias;

public class ListarCategoriasQueryHandler
    : IRequestHandler<ListarCategoriasQuery, List<CategoriaDto>>
{
    private readonly IApplicationDbContext _context;

    public ListarCategoriasQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> Handle(
        ListarCategoriasQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categorias
            .Select(c => new CategoriaDto(c.Id, c.Nombre))
            .ToListAsync(cancellationToken);
    }
}
