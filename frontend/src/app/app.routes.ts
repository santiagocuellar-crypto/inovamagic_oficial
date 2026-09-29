import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: 'registro',
    loadComponent: () =>
      import('./features/auth/register/register').then((m) => m.Register),
  },
  {
    path: '',
    loadComponent: () =>
      import('./features/auth/login/login').then((m) => m.Login),
  },
];
