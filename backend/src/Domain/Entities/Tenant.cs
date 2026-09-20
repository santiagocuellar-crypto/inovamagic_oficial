using ProyectoGrado.Domain.Common;

namespace ProyectoGrado.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
