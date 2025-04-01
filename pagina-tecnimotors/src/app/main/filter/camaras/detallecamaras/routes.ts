import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./detallecamaras.component').then(
        (m) => m.DetallecamarasComponent
      ),
    data: {
      title: 'Detalle Camara',
    },
  },
];
