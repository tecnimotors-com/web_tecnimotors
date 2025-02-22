import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./detallevehiculo.component').then(
        (m) => m.DetallevehiculoComponent
      ),
    data: {
      title: 'Detalle Vehiculos',
    },
  },
];
