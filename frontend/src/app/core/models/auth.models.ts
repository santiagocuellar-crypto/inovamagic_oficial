export interface RegisterRequest {
  nombreTenant: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
}

export interface UsuarioActual {
  usuarioId: string;
  email: string;
  tenantId: string;
  roles: string[];
}
