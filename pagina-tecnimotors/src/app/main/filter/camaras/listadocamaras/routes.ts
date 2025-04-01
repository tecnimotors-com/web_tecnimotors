import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./listadocamaras.component').then(
        (m) => m.ListadocamarasComponent
      ),
    data: {
      title: 'Listado Camara',
    },
  },
];
