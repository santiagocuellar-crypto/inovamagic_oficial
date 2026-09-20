namespace ProyectoGrado.Application.Features.Productos.Queries.ListarProductos;

public record ProductoListadoDto(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    string? CategoriaNombre,
    List<string> ImagenesUrls,
    List<VarianteDto> Variantes);

public record VarianteDto(string Talla, int Stock);
