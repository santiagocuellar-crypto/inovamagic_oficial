import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../config/api-config';
import { CategoriaDto, PedidoDto, ESTADOS } from '../models/admin.models';
import { ProductoPublicoDto } from '../models/catalogo.models';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private http = inject(HttpClient);

  listarCategorias(): Observable<CategoriaDto[]> {
    return this.http.get<CategoriaDto[]>(`${API_URL}/categorias`);
  }

  crearCategoria(nombre: string): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${API_URL}/categorias`, { nombre });
  }

  listarProductos(tenantId: string): Observable<ProductoPublicoDto[]> {
    return this.http.get<ProductoPublicoDto[]>(`${API_URL}/productos/publico/${tenantId}`);
  }

  crearProducto(body: {
    nombre: string;
    descripcion: string;
    precio: number;
    categoriaId: string | null;
    variantes: { talla: string; stock: number }[];
  }): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${API_URL}/productos`, body);
  }

  subirImagen(productoId: string, archivo: File): Observable<{ url: string }> {
    const datos = new FormData();
    datos.append('archivo', archivo);
    return this.http.post<{ url: string }>(`${API_URL}/productos/${productoId}/imagenes`, datos);
  }

  listarPedidos(): Observable<PedidoDto[]> {
    return this.http.get<PedidoDto[]>(`${API_URL}/pedidos`);
  }

  actualizarEstado(pedidoId: string, estado: string): Observable<void> {
    return this.http.put<void>(`${API_URL}/pedidos/${pedidoId}/estado`, {
      nuevoEstado: ESTADOS.indexOf(estado),
    });
  }
}
