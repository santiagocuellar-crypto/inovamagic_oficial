using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class Categoria : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
