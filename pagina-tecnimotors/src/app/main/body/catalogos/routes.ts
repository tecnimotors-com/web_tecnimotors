import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./catalogos.component').then((m) => m.CatalogosComponent),
    data: {
      title: 'Catálogo',
    },
  },
];
