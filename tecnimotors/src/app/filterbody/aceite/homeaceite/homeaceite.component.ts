import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';

@Component({
  selector: 'app-homeaceite',
  templateUrl: './homeaceite.component.html',
  styleUrls: ['./homeaceite.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeaceiteComponent implements OnInit, OnDestroy {
  public txttipoVehiculo: string | null = '';
  public txtcategiescamara: string = '';
  public txtListCategoriesCamera: any[] = [];

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
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.txttipoVehiculo = params['tipo'].toString();
    });
    this.voidiniciar();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  voidiniciar() {
    this.txtcategiescamara = this.txttipoVehiculo?.toString() || '0';

    this.servicesmaestro.getListCategorieAceite().subscribe({
      next: (dtl: any) => {
        this.ListModelCamaraALl();
        this.txtListCategoriesCamera = dtl;
        this.ListadoCamaraGeneral();
      },
    });
  }
  ListModelCamaraALl() {
    this.servicesmaestro.getListModeloAceite(this.txtcategiescamara).subscribe({
      next: (value: any) => {
        this.listmodelo = value;
      },
    });
  }
  getLastFiveDigits(codigo: string): string {
    return codigo.length > 5 ? codigo.substring(codigo.length - 5) : codigo;
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

  ChangeCategorieCamara() {
    this.router.navigate([`/homeaceite/${this.txtcategiescamara}`]);
    this.ListModelCamaraALl();
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

  ListadoCamaraGeneral() {
    this.loading = true;
    this.servicesmaestro
      .getListadoAceiteGeneral(this.txtcategiescamara)
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
    this.txtcategiescamara = '0';
    this.router.navigate([`/homeaceite/${this.txtcategiescamara}`]);
    this.ListModelCamaraALl();
    this.ListadoCamaraGeneral();
  }
  BtnRouterCamara(id: any) {
    this.router.navigateByUrl(`/detalleaceite/${id}`);
  }
}
