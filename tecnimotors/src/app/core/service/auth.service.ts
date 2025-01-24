import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment.prod';
import { RrhhService } from './rrhh.service';
import { DepartamentoService } from './departamento.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(
    this.checkAuthStatus()
  );
  private selectId: string | null = null;
  private selectTipo: string | null = null;

  constructor(
    private router: Router,
    private rrhhService: RrhhService,
    private departamentoservice: DepartamentoService
  ) {}

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
          this.router.navigate(['/homerepuesto/BICICLETA']);
        } else if (selectTipo === '2') {
          this.router.navigate(['/homerepuesto/MOTOCICLETA']);
        } else {
          this.router.navigate(['/homerepuesto/MOTOCICLETA']);
        }
        break;
      case '4':
        if (selectTipo === '2') {
          this.router.navigate(['/homeaceite/98']);
        } else {
          this.router.navigate(['/homeaceite/98']);
        }
        break;
      case '5':
        /*
        if (selectTipo === '1') {
          this.router.navigate(['/homevehiculo'], {
            queryParams: { tipo: 'Bicicletas' },
          });
        } else*/
        if (selectTipo === '2') {
          this.router.navigate(['/homevehiculo/MOTOCI']);
        } else if (selectTipo === '3') {
          this.router.navigate(['/homevehiculo/CUATRI']);
        } else if (selectTipo === '4') {
          this.router.navigate(['/homevehiculo/TRIMO']);
        } else {
          this.router.navigate(['/homevehiculo/MOTOCI']);
        }
        break;
      default:
        // No hacer nada si no se activa ninguna ruta
        break;
    }
  }

  getRefreshToken() {
    this.registrarTokenRRHH();
    this.registrarTokenDepartamento();
  }

  registrarTokenRRHH() {
    const tokenrrhh: any = {
      userName: environment.userrrhh,
      password: environment.passwordrrhh,
    };
    this.rrhhService.getRegistrarRRHH(tokenrrhh).subscribe((data: any) => {
      sessionStorage.setItem('tokenrrhh', data.tokenrrhh);
    });
  }

  registrarTokenDepartamento() {
    const tokendepas: any = {
      userName: environment.userdepa,
      password: environment.passworddepa,
    };
    this.departamentoservice
      .getRegistrarDepartamento(tokendepas)
      .subscribe((data) => {
        sessionStorage.setItem('tokendepa', data.tokendepa);
      });
  }
}
