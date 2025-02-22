//nosotros
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./cotizacion.component').then((m) => m.CotizacionComponent),
    data: {
      title: 'Cotización',
    },
  },
];
