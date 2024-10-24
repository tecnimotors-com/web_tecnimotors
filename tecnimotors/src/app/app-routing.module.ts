import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard2 } from './core/guard/auth.guard';
import { ProtectedComponent } from './body/protected/protected.component';
import { LoginComponent } from './auth/login/login.component';
import { HomeComponent } from './home/home.component';

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
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })], // Habilitar el modo hash
  exports: [RouterModule], // Exporta RouterModule para que esté disponible en otros módulos
})
export class AppRoutingModule {}
