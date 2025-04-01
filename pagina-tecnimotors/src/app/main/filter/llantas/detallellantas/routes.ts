import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./detallellantas.component').then(
        (m) => m.DetallellantasComponent
      ),
    data: {
      title: 'Detalle Llantas',
    },
  },
];
