import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { RrhhService } from './rrhh.service';
import { DepartamentoService } from './departamento.service';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';
import CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private key = environment.apikeencriptado;
  private myapitecni = environment.apimaestroarticulo;
  private isLoggedInSubject = new BehaviorSubject<boolean>(
    this.checkAuthStatus()
  );

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(
    this.isAuthenticatedcliente()
  );
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  private encrypt(data: string): string {
    if (!data) {
      throw new Error('Data to encrypt cannot be null or undefined');
    }
    return CryptoJS.AES.encrypt(data, this.key).toString();
  }

  private decrypt(data: string): string {
    if (!data) {
      return '';
    }
    const bytes = CryptoJS.AES.decrypt(data, this.key);
    return bytes.toString(CryptoJS.enc.Utf8);
  }

  constructor(
    private router: Router,
    private rrhhService: RrhhService,
    private departamentoservice: DepartamentoService,
    private http: HttpClient
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
          this.router.navigate(['/homerepuesto']);
        } else if (selectTipo === '2') {
          this.router.navigate(['/homerepuesto']);
        } else {
          this.router.navigate(['/homerepuesto']);
        }
        break;
      case '4':
        if (selectTipo === '2') {
          this.router.navigate(['/homeaceite']);
        } else {
          this.router.navigate(['/homeaceite']);
        }
        break;
      case '5':
        this.router.navigate(['/homevehiculo/' + selectTipo]);

        break;
      default:
        // No hacer nada si no se activa ninguna ruta
        break;
    }
  }

  getRefreshToken() {
    this.registrarTokenRRHH();
    this.registrarTokenDepartamento();
    localStorage.removeItem('marcavehiculo');
    localStorage.removeItem('modelovehiculo');
    localStorage.removeItem('pvehiculo');
    localStorage.removeItem('categoriavehiculo');
  }
  getRefreshTokenvehiculo() {
    localStorage.removeItem('returncategory');
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

  /*------------------------------------------------------*/
  registercliente(user: any): Observable<any> {
    return this.http.post(`${this.myapitecni}/Auth/register`, user);
  }

  // Método para guardar el usuario en localStorage
  saveUsercliente(correo: any, rememberMe: any, uuid: any) {
    const encrypteduuid = this.encrypt(uuid);
    localStorage.setItem('correo', correo);
    localStorage.setItem('rememberMe', rememberMe);
    localStorage.setItem('uuid', encrypteduuid);
  }

  // Método para obtener el usuario de localStorage
  getUsercliente() {
    localStorage.getItem('uuid');
    localStorage.getItem('rememberMe');
    return localStorage.getItem('correo');
  }

  // Método para verificar si el usuario está autenticado
  isAuthenticatedcliente(): boolean {
    localStorage.getItem('uuid');
    localStorage.getItem('rememberMe');
    return !!localStorage.getItem('correo');
  }

  logincliente(user: { correo: string; password: string }): Observable<any> {
    return this.http.post(`${this.myapitecni}/Auth/login`, user).pipe(
      tap(() => {
        this.isAuthenticatedSubject.next(true); // Actualizar el estado de autenticación
      })
    );
  }

  getDesconvertir() {
    var txtuuid = localStorage.getItem('uuid') ?? '';
    var result = txtuuid == '' ? txtuuid : this.decrypt(txtuuid);
    return result;
  }

  logoutcliente() {
    localStorage.removeItem('rememberMe');
    localStorage.removeItem('correo');
    localStorage.removeItem('uuid');
    this.isAuthenticatedSubject.next(false); // Actualizar el estado de autenticación
    this.showAlert('Se cerro sesion del cliente', 'info');
  }

  // type AlertType = 'success' | 'error' | 'info';
  showAlert(texto: string, type: any) {
    Swal.fire({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 1500,
      timerProgressBar: true,
      title: texto,
      icon: type,
    });
  }
}
