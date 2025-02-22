//nosotros
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./registromayorista.component').then(
        (m) => m.RegistromayoristaComponent
      ),
    data: {
      title: 'registro mayorista',
    },
  },
];
