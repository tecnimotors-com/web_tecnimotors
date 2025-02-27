import { inject, Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { take, tap } from 'rxjs/operators';
import { AuthService } from '../../core/service/auth.service';

export const authGuard2 = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.isLoggedIn().pipe(
    take(1),
    tap((isLoggedIn: any) =>
      isLoggedIn ? router.createUrlTree(['/protected']) : true
    )
  );
};


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
      if (this.authService.isAuthenticatedcliente()) {
          return true;
      } else {
          this.router.navigate(['/login']); // Redirige a la página de inicio de sesión
          return false;
      }
  }
}
