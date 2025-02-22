import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./categoria.component').then((m) => m.CategoriaComponent),
    data: {
      title: 'Categoria',
    },
  },
];
