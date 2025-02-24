//nosotros
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./wishlist.component').then((m) => m.WishlistComponent),
    data: {
      title: 'Wish List',
    },
  },
];
