import {
  Component,
  OnDestroy,
  OnInit,
  AfterViewInit,
  ViewEncapsulation,
  ElementRef,
  VERSION,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';
import lgZoom from 'lightgallery/plugins/zoom';
import { BeforeSlideDetail } from 'lightgallery/lg-events';
import { AuthService } from '../../../core/service/auth.service';

@Component({
  selector: 'app-detallellanta',
  templateUrl: './detallellanta.component.html',
  styleUrls: ['./detallellanta.component.css'],

  encapsulation: ViewEncapsulation.None,
})
export class DetallellantaComponent implements OnInit, OnDestroy {
  /*
  public swiper: any;

  public swiper2: any;
  
  ngAfterViewInit() {
    this.swiper = new Swiper('.swiper', {
      direction: 'vertical',
      slidesPerView: 3,
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
  */

  public id: number = 0;

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

  public srcimg: string = '';
  public baseurl: string = '../../../assets/img/Imagen/';
  public count: number = 1;
  public blndisable = false;

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private auth: AuthService
  ) {}

  settings = {
    counter: false,
    plugins: [lgZoom],
  };
  imageExists(imageUrl: string): boolean {
    const img = new Image();
    img.src = imageUrl;
    return img.complete;
  }
  onBeforeSlide = (detail: BeforeSlideDetail): void => {
    const { index, prevIndex } = detail;
    console.log(index, prevIndex);
  };

  ngOnInit(): void {
    this.auth.getRefreshToken();
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

        this.srcimg =
          this.baseurl + this.Dtlcodigoimg + '/' + this.Dtlcodigoimg + '_1.jpg';
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

  DtlImagen1() {
    this.srcimg =
      this.baseurl + this.Dtlcodigoimg + '/' + this.Dtlcodigoimg + '_1.jpg';
  }

  DtlImagen2() {
    this.srcimg =
      this.baseurl + this.Dtlcodigoimg + '/' + this.Dtlcodigoimg + '_2.jpg';
  }

  DtlImagen3() {
    this.srcimg =
      this.baseurl + this.Dtlcodigoimg + '/' + this.Dtlcodigoimg + '_3.jpg';
  }

  AumentarCount() {
    if (this.count < 10) {
      this.blndisable = false;
      this.count++;
    } else {
      this.blndisable = true;
      this.count = 10;
    }
  }

  RestarCount() {
    if (this.count <= 1) {
      this.count = 1;
    } else {
      this.count--;
    }
  }
}
