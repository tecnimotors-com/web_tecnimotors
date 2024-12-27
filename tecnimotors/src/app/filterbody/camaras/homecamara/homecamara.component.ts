import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';

@Component({
  selector: 'app-homecamara',
  templateUrl: './homecamara.component.html',
  styleUrls: ['./homecamara.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class HomecamaraComponent implements OnInit, OnDestroy {
  public txttipoVehiculo: string | null = '';
  txtmarca: string | null = '';

  public txtcategiescamara: string = '';
  public txtListCategoriesCamera: any[] = [];
  public txtinpumarca: string = '';

  public ListMarca: any[] = [
    {
      txt: 'DUNLOP',
    },
    {
      txt: 'KENDA',
    },
    {
      txt: 'CELIMO',
    },
    {
      txt: 'CS',
    },
  ];

  public listmodelo: any[] = [];
  public txtmodelo: string = '';

  public listCamaraGene: any[] = [];
  public txtcamaragene: string = '';
  p: number = 1;
  itemper: number = 16;
  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private router: Router
  ) {}

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

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.txttipoVehiculo = params['tipo'].toString();
      this.txtmarca = params['marca'].toString();
      console.log('Tipo de vehículo recibido:', this.txttipoVehiculo);
      console.log('Marca :', this.txtmarca);
    });
    this.voidiniciar();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  voidiniciar() {
    this.txtcategiescamara = this.txttipoVehiculo?.toString() || '0';
    this.txtinpumarca = this.txtmarca?.toString() || '0';

    this.servicesmaestro.getListCategorieCamara().subscribe({
      next: (dtl: any) => {
        this.ListModelCamaraALl();
        this.txtListCategoriesCamera = dtl;
        this.ListadoCamaraGeneral();
      },
    });
  }

  ChangeCategorieCamara() {
    // Actualiza los parámetros de la ruta

    this.router.navigate([
      `/homecamara/${this.txtcategiescamara}/${this.txtmarca}`,
    ]);
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }

  inputMarca() {
    this.router.navigate([
      `/homecamara/${this.txtcategiescamara}/${this.txtinpumarca}`,
    ]);
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }

  limpiarbtn() {
    this.txtcategiescamara = '0';
    this.txtinpumarca = '0';
    this.router.navigate([
      `/homecamara/${this.txtcategiescamara}/${this.txtinpumarca}`,
    ]);
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }

  ListModelCamaraALl() {
    this.servicesmaestro
      .getListModeloCamara(this.txtcategiescamara, this.txtinpumarca!)
      .subscribe({
        next: (value: any) => {
          this.listmodelo = value;
        },
      });
  }

  ListadoCamaraGeneral() {
    this.servicesmaestro
      .getListadoCamaraGeneral(this.txtcategiescamara, this.txtinpumarca!)
      .subscribe({
        next: (value: any) => {
          this.listCamaraGene = value;
        },
      });
  }
  SelectModelo() {}
}
