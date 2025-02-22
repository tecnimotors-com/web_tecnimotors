//nosotros
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./cuentabancaria.component').then(
        (m) => m.CuentabancariaComponent
      ),
    data: {
      title: 'Cuenta Bancaria',
    },
  },
];
