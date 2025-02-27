import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./libroreclamo.component').then((m) => m.LibroreclamoComponent),
    data: {
      title: 'libro reclamo',
    },
  },
];
