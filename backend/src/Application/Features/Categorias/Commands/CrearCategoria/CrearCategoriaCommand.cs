using MediatR;

namespace ProyectoGrado.Application.Features.Categorias.Commands.CrearCategoria;

public record CrearCategoriaCommand(string Nombre) : IRequest<Guid>;
