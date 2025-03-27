import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./detallerepuesto.component').then(
        (m) => m.DetallerepuestoComponent
      ),
    data: {
      title: 'Detalle Repuesto',
    },
  },
];
