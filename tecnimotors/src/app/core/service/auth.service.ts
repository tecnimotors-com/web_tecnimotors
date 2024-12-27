import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(
    this.checkAuthStatus()
  );
  private selectId: string | null = null;
  private selectTipo: string | null = null;

  constructor(private router: Router) {}

  login(username: string, password: string): Observable<boolean> {
    // Simula la autenticación
    const isLoggedIn = username === 'admin' && password === 'password';
    if (isLoggedIn) {
      localStorage.setItem('isLoggedIn', 'true'); // Guardar el estado en localStorage
    }
    this.isLoggedInSubject.next(isLoggedIn);
    return this.isLoggedInSubject.asObservable();
  }

  logout(): void {
    localStorage.removeItem('isLoggedIn'); // Eliminar el estado de localStorage
    this.isLoggedInSubject.next(false);
  }

  isLoggedIn(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  private checkAuthStatus(): boolean {
    // Verifica el estado de autenticación desde localStorage
    return localStorage.getItem('isLoggedIn') == 'true';
  }

  navigate(selectId: string, selectTipo: string, productmarca: string): void {
    console.log(productmarca);
    switch (selectId) {
      case '1':
        this.router.navigateByUrl('/homellantas');
        break;
      case '2':
        if (selectTipo === '1') {
          this.router.navigate(['/homecamara/06/' + productmarca]);
        } else if (selectTipo === '2') {
          this.router.navigate(['/homecamara/07/' + productmarca]);
        } else {
          this.router.navigate(['/homecamara/0/' + productmarca]);
        }
        break;
      case '3':
        if (selectTipo === '1') {
          this.router.navigate(['/homerepuesto'], {
            queryParams: { tipo: 'Bicicletas' },
          });
        } else if (selectTipo === '2') {
          this.router.navigate(['/homerepuesto'], {
            queryParams: { tipo: 'Motocicletas' },
          });
        } else {
          this.router.navigate(['/homerepuesto']);
        }
        break;
      case '4':
        if (selectTipo === '2') {
          this.router.navigate(['/homeaceite'], {
            queryParams: { tipo: 'Motocicletas' },
          });
        }
        break;
      case '5':
        if (selectTipo === '1') {
          this.router.navigate(['/homevehiculo'], {
            queryParams: { tipo: 'Bicicletas' },
          });
        } else if (selectTipo === '2') {
          this.router.navigate(['/homevehiculo'], {
            queryParams: { tipo: 'Motocicletas' },
          });
        } else if (selectTipo === '3') {
          this.router.navigate(['/homevehiculo'], {
            queryParams: { tipo: 'Cuatrimotos' },
          });
        } else if (selectTipo === '4') {
          this.router.navigate(['/homevehiculo'], {
            queryParams: { tipo: 'Cargueros' },
          });
        }
        break;
      default:
        // No hacer nada si no se activa ninguna ruta
        break;
    }
  }
}
