import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_URL } from '../config/api-config';
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  UsuarioActual,
} from '../models/auth.models';

const TOKEN_KEY = 'proyecto_grado_token';
const ROLE_CLAIM =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

@Injectable({ providedIn: 'root' })
export class AuthService {
  usuarioActual = signal<UsuarioActual | null>(this.leerUsuarioDelToken());
  estaLogueado = computed(() => this.usuarioActual() !== null);

  constructor(private http: HttpClient) {}

  registrar(datos: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_URL}/auth/register`, datos)
      .pipe(tap((res) => this.guardarSesion(res.token)));
  }

  login(datos: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_URL}/auth/login`, datos)
      .pipe(tap((res) => this.guardarSesion(res.token)));
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    this.usuarioActual.set(null);
  }

  obtenerToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private guardarSesion(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
    this.usuarioActual.set(this.decodificarToken(token));
  }

  private leerUsuarioDelToken(): UsuarioActual | null {
    const token = this.obtenerToken();
    if (!token) return null;
    return this.decodificarToken(token);
  }

  private decodificarToken(token: string): UsuarioActual | null {
    try {
      const payloadBase64 = token.split('.')[1];
      const payload = JSON.parse(atob(payloadBase64));

      const rolesRaw = payload[ROLE_CLAIM];
      const roles = Array.isArray(rolesRaw)
        ? rolesRaw
        : rolesRaw
          ? [rolesRaw]
          : [];

      return {
        usuarioId: payload.sub,
        email: payload.email,
        tenantId: payload.tenantId,
        roles,
      };
    } catch {
      return null;
    }
  }
}
