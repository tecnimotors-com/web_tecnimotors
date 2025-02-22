import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./blog.component').then((m) => m.BlogComponent),
    data: {
      title: 'Blog',
    },
  },
];
