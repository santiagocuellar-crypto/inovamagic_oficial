import { Component, inject } from '@angular/core';
import { Router, RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-panel-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="shell">
      <aside class="rail">
        <div class="marca">✳ Mi Tienda</div>
        <nav>
          <a routerLink="/panel/productos" routerLinkActive="activo">Productos</a>
          <a routerLink="/panel/pedidos" routerLinkActive="activo">Pedidos</a>
          <a routerLink="/panel/categorias" routerLinkActive="activo">Categorías</a>
        </nav>
        <div class="pie">
          @if (auth.usuarioActual(); as u) {
            <a class="ver-tienda" [href]="'/tienda/' + u.tenantId" target="_blank">Ver mi tienda ↗</a>
            <span class="correo">{{ u.email }}</span>
          }
          <button type="button" (click)="salir()">Cerrar sesión</button>
        </div>
      </aside>
      <main><router-outlet /></main>
    </div>
  `,
  styles: [`
    .shell { display: flex; min-height: 100vh; background: #e9eaed; }
    .rail { width: 230px; background: #26262b; color: #fff; padding: 1.4rem 1rem; display: flex; flex-direction: column; gap: 1.5rem; }
    .marca { font-weight: 700; font-size: 1.05rem; padding: 0 .5rem; }
    nav { display: flex; flex-direction: column; gap: .3rem; flex: 1; }
    nav a { color: #b8b8c2; text-decoration: none; padding: .65rem .8rem; border-radius: 10px; font-size: .92rem; }
    nav a:hover { background: #33333a; color: #fff; }
    nav a.activo { background: #3d3d46; color: #fff; font-weight: 600; }
    .pie { display: flex; flex-direction: column; gap: .5rem; font-size: .8rem; }
    .ver-tienda { color: #a5b4fc; text-decoration: none; font-weight: 600; }
    .correo { color: #8b8b96; word-break: break-all; }
    .pie button { background: #33333a; color: #fff; border: none; border-radius: 8px; padding: .55rem; cursor: pointer; }
    main { flex: 1; padding: 2rem; background: #f5f6f8; margin: .6rem; border-radius: 18px; overflow-y: auto; }
  `],
})
export class PanelLayout {
  auth = inject(AuthService);
  private router = inject(Router);

  salir(): void {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }
}
