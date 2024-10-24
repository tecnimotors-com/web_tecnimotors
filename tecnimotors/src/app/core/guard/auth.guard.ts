import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { take, tap } from 'rxjs/operators';
import { AuthService } from '../../core/service/auth.service';

/*
@Injectable({
  providedIn: 'root',
})
*/

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
/*
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
      if(this.authService.isLoggedIn()){
        return true;
      }else{
        this.router.navigate(['/login']);
        return false; 
      }
  
  }
}
*/
