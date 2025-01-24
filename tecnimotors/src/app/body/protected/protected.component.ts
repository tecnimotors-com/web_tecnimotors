import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/service/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-protected',
  templateUrl: './protected.component.html',
  styleUrl: './protected.component.css',
  standalone: false,
})
export class ProtectedComponent implements OnInit {
  public islogin: string = '';

  constructor(private authservice: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.islogin = localStorage.getItem('isLoggedIn')!;
  }
  /*
  logout() {
    this.authservice.logout();
    this.router.navigate(['home']);
  }
  */
  async logout() {
    this.authservice.logout(); // Asegúrate de que el logout esté completo
    this.router.navigate(['home']);
  }
}
