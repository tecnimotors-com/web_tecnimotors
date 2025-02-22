//nosotros
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./distribuidores.component').then(
        (m) => m.DistribuidoresComponent
      ),
    data: {
      title: 'Distribuidores',
    },
  },
];
