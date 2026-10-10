import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../config/api-config';
import { CrearPedidoRequest, CrearPedidoResponse } from '../models/pedido.models';

@Injectable({ providedIn: 'root' })
export class PedidosService {
  private http = inject(HttpClient);

  crear(datos: CrearPedidoRequest): Observable<CrearPedidoResponse> {
    return this.http.post<CrearPedidoResponse>(`${API_URL}/pedidos`, datos);
  }
}
