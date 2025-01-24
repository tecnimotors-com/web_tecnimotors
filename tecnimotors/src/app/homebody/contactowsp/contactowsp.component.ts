import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../core/service/auth.service';
import { RrhhService } from '../../core/service/rrhh.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-contactowsp',
  templateUrl: './contactowsp.component.html',
  styleUrls: ['./contactowsp.component.css'],
  standalone: false,
})
export class ContactowspComponent implements OnInit, OnDestroy {
  public ListVendedor: any[] = [];

  private initializePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');

    if (preloaderWrapper) {
      preloaderWrapper.classList.remove('loaded');

      setTimeout(() => {
        preloaderWrapper.classList.add('loaded');
      }, 1000);
    } else {
      console.error('Preloader not found!');
    }
  }

  private finalizePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');
    if (preloaderWrapper) {
      preloaderWrapper.classList.add('loaded');
    }
  }

  constructor(
    private auth: AuthService,
    private rrhh: RrhhService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();

    this.ListadoContactoVendedor();
  }
  formatCelular(celular: string): string {
    return celular.replace(/\D/g, '');
  }
  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  ListadoContactoVendedor() {
    this.rrhh.getListaVendedorPublico().subscribe({
      next: (value: any) => {
        this.ListVendedor = value;
      },
    });
  }
}
