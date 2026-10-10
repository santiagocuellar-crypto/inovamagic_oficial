import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: 'registro',
    loadComponent: () => import('./features/auth/register/register').then((m) => m.Register),
  },
  {
    path: 'panel',
    canActivate: [authGuard],
    loadComponent: () => import('./features/panel/panel-layout').then((m) => m.PanelLayout),
    children: [
      { path: '', redirectTo: 'productos', pathMatch: 'full' },
      {
        path: 'productos',
        loadComponent: () => import('./features/panel/productos/productos').then((m) => m.Productos),
      },
      {
        path: 'pedidos',
        loadComponent: () => import('./features/panel/pedidos/pedidos').then((m) => m.Pedidos),
      },
      {
        path: 'categorias',
        loadComponent: () => import('./features/panel/categorias/categorias').then((m) => m.Categorias),
      },
    ],
  },
  {
    path: 'tienda/:tenantId',
    loadComponent: () => import('./features/tienda/catalogo/catalogo').then((m) => m.Catalogo),
  },
  {
    path: 'tienda/:tenantId/carrito',
    loadComponent: () => import('./features/tienda/carrito/carrito').then((m) => m.Carrito),
  },
  {
    path: 'tienda/:tenantId/checkout',
    loadComponent: () => import('./features/tienda/checkout/checkout').then((m) => m.Checkout),
  },
  {
    path: '',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
  },
];
