using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class Pedido : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;
    public string ClienteTelefono { get; set; } = string.Empty;
    public string? ClienteEmail { get; set; }

    public string DireccionCalle { get; set; } = string.Empty;
    public string DireccionCiudad { get; set; } = string.Empty;
    public string? DireccionNotas { get; set; }

    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;
    public decimal Total { get; set; }

    public List<PedidoItem> Items { get; set; } = new();
}
