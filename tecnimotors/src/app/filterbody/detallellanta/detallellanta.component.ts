import {
  Component,
  OnDestroy,
  OnInit,
  AfterViewInit,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MaestroarticuloService } from '../../core/service/maestroarticulo.service';
import Swiper from 'swiper';
import { Navigation, Pagination, Scrollbar } from 'swiper/modules';

@Component({
  selector: 'app-detallellanta',
  templateUrl: './detallellanta.component.html',
  styleUrls: ['./detallellanta.component.css'],

  encapsulation: ViewEncapsulation.None,
})
export class DetallellantaComponent implements OnInit, OnDestroy {
  ngAfterViewInit() {
    // Now you can use Swiper
    this.swiper = new Swiper('.swiper', {
      slidesPerGroupSkip: 4,
      keyboard: {
        enabled: true,
      },
      breakpoints: {
        769: {
          slidesPerView: 4,
          slidesPerGroup: 4,
        },
      },
      slidesPerView: 1,
      centeredSlides: false,
      grabCursor: true,
      modules: [Navigation, Pagination, Scrollbar],
      speed: 500,
    });

    this.swiper2 = new Swiper('.swiperfiel', {
      slidesPerGroupSkip: 1,
      keyboard: {
        enabled: true,
      },
      slidesPerView: 1,
      centeredSlides: false,
      grabCursor: true,
      modules: [Navigation, Pagination, Scrollbar],
      speed: 500,
      pagination: {
        el: '.swiperpagina',
        type: 'fraction',
        clickable: true,
      },
    });
  }

  public id: number = 0;

  public swiper: any;

  public swiper2: any;

  public Dtlid: number = 0;
  public Dtlcodigoimg: string = '';
  public Dtlcodigo: string = '';
  public Dtldescripcion: string = '';
  public Dtlunidad_medida: string = '';
  public Dtlmarca: string = '';
  public Dtlabreviado: string = '';
  public Dtlcodigo_equivalente: string = '';
  public Dtlmarcaoriginal: string = '';
  public Dtlcocada: string = '';
  public Dtlancho: string = '';
  public Dtlperfil: string = '';
  public Dtlaro: string = '';
  public Dtltipouso: string = '';
  public Dtlestado: string = '';

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService
  ) {}

  ngOnInit(): void {
    this.DetailArticulo();

    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  DetailArticulo() {
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });

    this.servicesmaestro.getDetalleArticulo(this.id).subscribe({
      next: (dtl: any) => {
        console.log(dtl);
        this.Dtlid = dtl.id;
        this.Dtlcodigoimg = dtl.codigo;
        this.Dtlcodigo = dtl.codigo.slice(-5);
        this.Dtldescripcion = dtl.descripcion;
        this.Dtlunidad_medida = dtl.unidad_medida;
        this.Dtlmarca = dtl.marca;
        this.Dtlabreviado = dtl.abreviado;
        this.Dtlcodigo_equivalente = dtl.codigo_equivalente;
        this.Dtlmarcaoriginal = dtl.marcaoriginal;
        this.Dtlcocada = dtl.cocada;
        this.Dtlancho = dtl.ancho;
        this.Dtlperfil = dtl.perfil;
        this.Dtlaro = dtl.aro;
        this.Dtltipouso = dtl.tipouso;
        this.Dtlestado = dtl.estado;
      },
    });
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
}
