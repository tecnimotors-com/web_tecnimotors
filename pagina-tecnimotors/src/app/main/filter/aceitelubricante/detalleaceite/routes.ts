import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./detalleaceite.component').then(
        (m) => m.DetalleaceiteComponent
      ),
    data: {
      title: 'Detalle Aceites',
    },
  },
];
