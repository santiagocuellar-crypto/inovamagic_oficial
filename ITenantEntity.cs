namespace ProyectoGrado.Domain.Common;

// Cualquier entidad futura (Producto, Pedido, etc.) que deba estar aislada
// por tenant implementa esta interfaz. El DbContext usa esto para aplicar
// el filtro automatico de "solo ver mis propios datos".
public interface ITenantEntity
{
    Guid TenantId { get; set; }
}
