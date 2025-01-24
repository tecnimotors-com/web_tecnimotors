import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';
import { AuthService } from '../../../core/service/auth.service';

@Component({
  selector: 'app-homevehiculo',
  templateUrl: './homevehiculo.component.html',
  styleUrls: ['./homevehiculo.component.css'],
  standalone: false,
})
export class HomevehiculoComponent implements OnInit, OnDestroy {
  public txttipoVehiculo: string | null = '';
  public txtcategiescamara: string = '';
  public txtListCategoriesCamera: any[] = [
    {
      txt: 'MOTOCI',
      txt2: 'Motocicletas',
    },
    {
      txt: 'CUATRI',
      txt2: 'Cuatrimotos',
    },
    {
      txt: 'TRIMO',
      txt2: 'Cargueros',
    },
  ];

  public loading: boolean = false;

  p: number = 1;
  itemper: number = 12;

  public listmodelo: any[] = [];
  public txtmodelo: string = '';

  public listCamaraGene: any[] = [];
  public txtcamaragene: string = '';
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
    this.route.params.subscribe((params) => {
      this.txtcategiescamara = params['marca'].toString();
    });
    this.voidiniciar();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  voidiniciar() {
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }

  SelectModelo() {
    var modelo = this.txtmodelo ?? '';
    if (modelo == '') {
      this.ListadoCamaraGeneral();
    } else {
      this.servicesmaestro
        .getListadoCamaraGeneralModelo(this.txtmodelo)
        .subscribe({
          next: (value: any) => {
            this.listCamaraGene = value;
          },
          error: (err) => {
            console.error(err);
          },
          complete: () => {
            this.loading = false;
          },
        });
    }
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  ChangeCategorieCamara() {
    this.router.navigate([`/homevehiculo/${this.txtcategiescamara}`]);
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }

  ListModelCamaraALl() {
    this.servicesmaestro
      .getListModeloVehiculo(this.txtcategiescamara)
      .subscribe({
        next: (value: any) => {
          this.listmodelo = value;
        },
      });
  }

  ListadoCamaraGeneral() {
    this.loading = true;
    this.servicesmaestro
      .getListadoVehiculoGeneral(this.txtcategiescamara)
      .subscribe({
        next: (value: any) => {
          this.listCamaraGene = value;
        },
        error: (err) => {
          console.error(err);
        },
        complete: () => {
          this.loading = false;
        },
      });
  }

  limpiarbtn() {
    this.txtcategiescamara = 'MOTOCI';
    this.router.navigate([`/homevehiculo/${this.txtcategiescamara}`]);
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }

  BtnRouterCamara(id: any) {
    this.router.navigateByUrl(`/detallevehiculo/${id}`);
  }

  getLastFiveDigits(codigo: string): string {
    return codigo.length > 5 ? codigo.substring(codigo.length - 5) : codigo;
  }
}
