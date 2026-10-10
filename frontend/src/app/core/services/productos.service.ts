import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../config/api-config';
import { ProductoPublicoDto } from '../models/catalogo.models';

@Injectable({ providedIn: 'root' })
export class ProductosService {
  private http = inject(HttpClient);

  listarPublico(tenantId: string): Observable<ProductoPublicoDto[]> {
    return this.http.get<ProductoPublicoDto[]>(
      `${API_URL}/productos/publico/${tenantId}`,
    );
  }
}
