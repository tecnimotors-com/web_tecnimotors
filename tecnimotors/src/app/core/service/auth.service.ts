import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.checkAuthStatus());

  constructor() { }

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
}