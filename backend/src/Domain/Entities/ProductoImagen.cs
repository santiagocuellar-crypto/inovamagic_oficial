using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class ProductoImagen : BaseEntity
{
    public Guid ProductoId { get; set; }
    public Producto? Producto { get; set; }

    public string Url { get; set; } = string.Empty;
    public int Orden { get; set; }
}
