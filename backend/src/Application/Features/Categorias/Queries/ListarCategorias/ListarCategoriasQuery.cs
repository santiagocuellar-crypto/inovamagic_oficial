using MediatR;

namespace ProyectoGrado.Application.Features.Categorias.Queries.ListarCategorias;

public record CategoriaDto(Guid Id, string Nombre);

public record ListarCategoriasQuery : IRequest<List<CategoriaDto>>;
