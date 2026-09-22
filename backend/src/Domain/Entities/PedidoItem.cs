using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class PedidoItem : BaseEntity
{
    public Guid PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public Guid ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public string Talla { get; set; } = string.Empty;

    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
