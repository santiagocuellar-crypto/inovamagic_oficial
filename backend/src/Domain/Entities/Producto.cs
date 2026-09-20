using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class Producto : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public bool Activo { get; set; } = true;

    public Guid? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public List<ProductoVariante> Variantes { get; set; } = new();
    public List<ProductoImagen> Imagenes { get; set; } = new();
}
