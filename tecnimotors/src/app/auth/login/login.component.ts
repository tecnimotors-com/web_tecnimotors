import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/service/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'], // Corregido el typo en styleUrl
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin(): void {
    this.authService
      .login(this.username, this.password)
      .subscribe((isLoggedIn) => {
        if (isLoggedIn) {
          console.log(isLoggedIn);
          this.router.navigate(['/protected']); // Redirigir a la ruta protegida
        } else {
          this.errorMessage = 'Credenciales incorrectas. Intenta de nuevo.';
        }
      });
  }
}
