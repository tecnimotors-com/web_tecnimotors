import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard2 } from './core/guard/auth.guard';
import { ProtectedComponent } from './body/protected/protected.component';
import { LoginComponent } from './auth/login/login.component';
import { HomeComponent } from './home/home.component';
import { NosotrosComponent } from './homebody/nosotros/nosotros.component';
import { CategoriaComponent } from './homebody/categoria/categoria.component';
import { CotizacionComponent } from './homebody/cotizacion/cotizacion.component';
import { RegistromayoristaComponent } from './homebody/registromayorista/registromayorista.component';
import { CatalogosComponent } from './homebody/catalogos/catalogos.component';
import { BlogComponent } from './homebody/blog/blog.component';
// Importa las rutas desde app.routes.ts
export const routes: Routes = [
  {
    path: 'protected',
    component: ProtectedComponent,
    canActivate: [authGuard2], // Asegúrate de que el guardia esté correctamente importado
  },
  {
    path: 'login',
    component: LoginComponent, // Ruta pública
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full',
  },
  {
    path: 'nosotros',
    component: NosotrosComponent,
  },
  {
    path: 'categoria',
    component: CategoriaComponent,
  },
  {
    path: 'cotizacion',
    component: CotizacionComponent,
  },
  {
    path: 'mayorista',
    component: RegistromayoristaComponent,
  },
  {
    path: 'catalogo',
    component: CatalogosComponent,
  },
  {
    path: 'blog',
    component: BlogComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })], // Habilitar el modo hash
  exports: [RouterModule], // Exporta RouterModule para que esté disponible en otros módulos
})
export class AppRoutingModule {}
