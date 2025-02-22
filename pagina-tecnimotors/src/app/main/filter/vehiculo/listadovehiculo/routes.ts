import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./listadovehiculo.component').then(
        (m) => m.ListadovehiculoComponent
      ),
    data: {
      title: 'Listado Vehiculos',
    },
  },
];
