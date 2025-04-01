import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./listadollantas.component').then(
        (m) => m.ListadollantasComponent
      ),
    data: {
      title: 'Listado Llantas',
    },
  },
];
