export interface CategoriaDto {
  id: string;
  nombre: string;
}

export interface PedidoItemDto {
  productoNombre: string;
  talla: string;
  cantidad: number;
  precioUnitario: number;
}

export interface PedidoDto {
  id: string;
  clienteNombre: string;
  clienteTelefono: string;
  clienteEmail: string | null;
  direccionCalle: string;
  direccionCiudad: string;
  direccionNotas: string | null;
  estado: string;
  total: number;
  fechaCreacion: string;
  items: PedidoItemDto[];
}

// El orden importa: el backend espera el numero (0 = Pendiente, 1 = Confirmado...)
export const ESTADOS = ['Pendiente', 'Confirmado', 'Enviado', 'Entregado', 'Cancelado'];
