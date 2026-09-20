using MediatR;
using ProyectoGrado.Application.Common.Interfaces;
using ProyectoGrado.Domain.Entities;

namespace ProyectoGrado.Application.Features.Categorias.Commands.CrearCategoria;

public class CrearCategoriaCommandHandler : IRequestHandler<CrearCategoriaCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _currentTenantService;

    public CrearCategoriaCommandHandler(
        IApplicationDbContext context, ICurrentTenantService currentTenantService)
    {
        _context = context;
        _currentTenantService = currentTenantService;
    }

    public async Task<Guid> Handle(CrearCategoriaCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentTenantService.TenantId
            ?? throw new InvalidOperationException("No hay un tenant identificado para este usuario");

        var categoria = new Categoria
        {
            TenantId = tenantId,
            Nombre = request.Nombre
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync(cancellationToken);

        return categoria.Id;
    }
}
