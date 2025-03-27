import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./listadoaceite.component').then(
        (m) => m.ListadoaceiteComponent
      ),
    data: {
      title: 'Listado Aceites',
    },
  },
];
