import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./listadorepuesto.component').then(
        (m) => m.ListadorepuestoComponent
      ),
    data: {
      title: 'Listado Repuesto',
    },
  },
];
