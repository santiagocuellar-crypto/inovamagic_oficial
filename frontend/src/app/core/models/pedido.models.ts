export interface ItemCarrito {
  productoId: string;
  nombre: string;
  talla: string;
  precio: number;
  cantidad: number;
  imagenUrl: string | null;
  stockDisponible: number;
}

export interface CrearPedidoRequest {
  tenantId: string;
  clienteNombre: string;
  clienteTelefono: string;
  clienteEmail?: string;
  direccionCalle: string;
  direccionCiudad: string;
  direccionNotas?: string;
  items: { productoId: string; talla: string; cantidad: number }[];
}

export interface CrearPedidoResponse {
  id: string;
}
