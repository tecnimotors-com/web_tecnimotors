//nosotros
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./contactowsp.component').then((m) => m.ContactowspComponent),
    data: {
      title: 'contacto wsp',
    },
  },
];
