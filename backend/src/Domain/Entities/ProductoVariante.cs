using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class ProductoVariante : BaseEntity
{
    public Guid ProductoId { get; set; }
    public Producto? Producto { get; set; }

    public string Talla { get; set; } = "Unica";
    public int Stock { get; set; }
}
