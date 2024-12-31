import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';
import { AuthService } from '../../../core/service/auth.service';

@Component({
  selector: 'app-homerepuesto',
  templateUrl: './homerepuesto.component.html',
  styleUrls: ['./homerepuesto.component.css'],
})
export class HomerepuestoComponent implements OnInit, OnDestroy {
  public ListtCategoriesCamera: any[] = [
    {
      tipo_articulo: 'MOTOCICLETA',
      descripcion_tipo_articulo: 'Motocicleta',
    },
    {
      tipo_articulo: 'BICICLETA',
      descripcion_tipo_articulo: 'Bicicleta',
    },
  ];
  public txtcategiesrepuesto: string = '';

  public ListTipoCategoria: any[] = [];
  public txttipocategoria: string = '0';

  public listmodelo: any[] = [];
  public txtmodelo: string = '';

  public listrepuesto: any[] = [];
  p: number = 1;
  itemper: number = 12;

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private router: Router,
    private auth: AuthService
  ) {}

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

  ngOnInit(): void {
    this.auth.getRefreshToken();
    this.route.params.subscribe((params: any) => {
      this.txtcategiesrepuesto = params['tipo'];
    });
    this.VoidIniciar();

    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  VoidIniciar() {
    this.ListadoCategoria();
    this.ListModelRepuestoALl();
    this.ListadoRepuestoGeneral();
  }

  limpiarbtn() {
    this.txtmodelo = '';
    this.txttipocategoria = '0';
    this.txtcategiesrepuesto = 'MOTOCICLETA';
    this.router.navigate([`/homerepuesto/MOTOCICLETA`]);
    this.VoidIniciar();
  }

  ChangeCategoria() {
    this.txttipocategoria = '0';
    this.txtmodelo = '';
    this.router.navigate([`/homerepuesto/${this.txtcategiesrepuesto}`]);
    this.VoidIniciar();
  }

  ListadoCategoria() {
    this.servicesmaestro
      .getListadoRepuestoTipoCategoria(this.txtcategiesrepuesto)
      .subscribe({
        next: (value: any) => {
          this.ListTipoCategoria = value;
        },
      });
  }

  ChangeTipoCategoria() {
    this.txtmodelo = '';
    this.ListModelRepuestoALl();
    this.ListadoRepuestoGeneral();
  }

  SelectModel() {
    var modelo = this.txtmodelo ?? '';
    if (modelo == '') {
      this.ListModelRepuestoALl();
      this.ListadoRepuestoGeneral();
    } else {
      this.servicesmaestro
        .getListadoCamaraGeneralModelo(this.txtmodelo)
        .subscribe({
          next: (value: any) => {
            this.listrepuesto = value;
          },
          error: (err) => {
            console.error(err);
          },
          complete: () => {},
        });
    }
  }

  getLastFiveDigits(codigo: string): string {
    return codigo.length > 5 ? codigo.substring(codigo.length - 5) : codigo;
  }

  BtnRouterRepuesto(id: any) {
    this.router.navigateByUrl(`/detallerepuesto/${id}`);
  }

  ListadoRepuestoGeneral() {
    this.servicesmaestro
      .getListadoRepuestoGeneralALl(this.txttipocategoria)
      .subscribe({
        next: (value: any) => {
          this.listrepuesto = value;
        },
      });
  }
  ListModelRepuestoALl() {
    this.servicesmaestro
      .getListadoModeloRepuesto(this.txttipocategoria, this.txtcategiesrepuesto)
      .subscribe({
        next: (value: any) => {
          console.log(this.txttipocategoria);
          console.log(this.txtcategiesrepuesto);
          console.log(value);
          this.listmodelo = value;
        },
      });
  }
}
