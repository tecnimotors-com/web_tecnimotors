import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MainComponent } from './main/main.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'nosotros',
        loadChildren: () =>
          import('./main/body/nosotros/routes').then((m) => m.routes),
      },
      {
        path: 'mayorista',
        loadChildren: () =>
          import('./main/body/registromayorista/routes').then((m) => m.routes),
      },
      {
        path: 'cotizacion',
        loadChildren: () =>
          import('./main/body/cotizacion/router').then((m) => m.routes),
      },
      {
        path: 'contactowsp',
        loadChildren: () =>
          import('./main/body/contactowsp/routes').then((m) => m.routes),
      },
      {
        path: 'categoria',
        loadChildren: () =>
          import('./main/body/categoria/routes').then((m) => m.routes),
      },
      {
        path: 'catalogo',
        loadChildren: () =>
          import('./main/body/catalogos/routes').then((m) => m.routes),
      },
      {
        path: 'blog',
        loadChildren: () =>
          import('./main/body/blog/routes').then((m) => m.routes),
      },
      {
        path: 'bancariatecnimotors',
        loadChildren: () =>
          import('./main/body/cuentabancaria/routes').then((m) => m.routes),
      },
      {
        path: 'distribuidores',
        loadChildren: () =>
          import('./main/body/distribuidores/routes').then((m) => m.routes),
      },
      {
        path: 'wishlist',
        loadChildren: () =>
          import('./main/body/wishlist/routes').then((m) => m.routes),
      },

      /*---------------------Filter-------------------------*/
      {
        path: 'homevehiculo/:marca',
        loadChildren: () =>
          import('./main/filter/vehiculo/listadovehiculo/routes').then(
            (m) => m.routes
          ),
      },
      {
        path: 'detallevehiculo/:id',
        loadChildren: () =>
          import('./main/filter/vehiculo/detallevehiculo/routes').then(
            (m) => m.routes
          ),
      },
    ],
  },
];
