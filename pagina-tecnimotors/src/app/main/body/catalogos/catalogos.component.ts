import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { CatalogoService } from '../../../core/service/catalogo.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { SharedMain } from '../../sharedmain';

@Component({
  selector: 'app-catalogos',
  imports: [SharedMain],
  templateUrl: './catalogos.component.html',
  styleUrls: ['./catalogos.component.scss'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('500ms ease-in', style({ opacity: 1 })),
      ]),
      transition(':leave', [animate('500ms ease-out', style({ opacity: 0 }))]),
    ]),
  ],
})
export class CatalogosComponent implements OnInit, OnDestroy {
  public p: number = 1;
  public itemper: number = 12;

  public ListTipoCatalogo: any[] = [];
  public txttipocatalogo: number = 0;

  public ListCatalogo: any[] = [];

  constructor(
    private auth: AuthService,
    private catalogoservice: CatalogoService
  ) {}

  ngOnInit(): void {
    this.Inicializador();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  Inicializador() {
    this.auth.getRefreshToken();
    this.ListadoTipoCatalogo();
    this.ListadoCatalogo();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

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

  ListadoCatalogo() {
    if (this.txttipocatalogo == 0) {
      this.catalogoservice.getListarCatalogoCompleto().subscribe({
        next: (value: any) => {
          this.ListCatalogo = value;
        },
      });
    } else {
      this.catalogoservice
        .getListarFiltroTipoCatalogo(this.txttipocatalogo)
        .subscribe({
          next: (value: any) => {
            this.ListCatalogo = value;
          },
        });
    }
  }

  ListadoTipoCatalogo() {
    this.catalogoservice.getListarTipoCatalogo().subscribe({
      next: (value: any) => {
        this.ListTipoCatalogo = value;
      },
    });
  }

  SelectCatalogo() {
    this.ListadoCatalogo();
  }

  Limpiar() {
    this.txttipocatalogo = 0;
    this.catalogoservice.getListarCatalogoCompleto().subscribe({
      next: (value: any) => {
        this.ListCatalogo = value;
      },
    });
  }
}
