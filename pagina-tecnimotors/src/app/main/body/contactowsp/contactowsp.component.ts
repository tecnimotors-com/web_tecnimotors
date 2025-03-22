import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { RrhhService } from '../../../core/service/rrhh.service';
import { HttpClient } from '@angular/common/http';
import { SharedMain } from '../../sharedmain';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';

@Component({
  selector: 'app-contactowsp',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './contactowsp.component.html',
  styleUrls: ['./contactowsp.component.scss'],
})
export class ContactowspComponent implements OnInit, OnDestroy {
  public ListVendedor: any[] = [];

  constructor(private auth: AuthService, private rrhh: RrhhService) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();

    this.ListadoContactoVendedor();
  }
  formatCelular(celular: string): string {
    return celular.replace(/\D/g, '');
  }
  ngOnDestroy(): void {}

  ListadoContactoVendedor() {
    this.rrhh.getListaVendedorPublico().subscribe({
      next: (value: any) => {
        this.ListVendedor = value;
      },
    });
  }
}
