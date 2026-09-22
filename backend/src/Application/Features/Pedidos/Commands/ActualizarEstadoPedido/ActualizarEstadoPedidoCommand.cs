using MediatR;
using ProyectoGrado.Domain.Entities;

namespace ProyectoGrado.Application.Features.Pedidos.Commands.ActualizarEstadoPedido;

public record ActualizarEstadoPedidoCommand(Guid PedidoId, EstadoPedido NuevoEstado) : IRequest;
