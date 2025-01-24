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
import { CotizacionService } from '../../../core/service/cotizacion.service';

@Component({
  selector: 'app-detallellanta',
  templateUrl: './detallellanta.component.html',
  styleUrls: ['./detallellanta.component.css'],
  encapsulation: ViewEncapsulation.None,
  standalone: false,
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
  public DtlFamilia: string = '';
  public DtlSubFamilia: string = '';
  public DtlTipoArticulo: string = '';
  public DtlUnidadMedida: string = '';

  public srcimg: string = '';
  public baseurl: string = '../../../assets/img/Imagen/';
  public count: number = 1;
  public blndisable = false;
  public ListCarrito: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private auth: AuthService,
    private cotizacionService: CotizacionService
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
        this.DtlFamilia = dtl.familia;
        this.DtlSubFamilia = dtl.subfamilia;
        this.DtlTipoArticulo = dtl.descripcion_tipo_articulo;
        this.DtlUnidadMedida = dtl.unidad_medida;
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

  AgregarCarrito() {
    // Crear el producto que se va a agregar
    const product = {
      id: this.Dtlid,
      codigo: this.Dtlcodigoimg,
      descripcion: this.Dtldescripcion,
      familia: this.DtlFamilia,
      subfamilia: this.DtlSubFamilia,
      marca: this.Dtlmarca,
      tipo: this.DtlTipoArticulo,
      unidad: this.DtlUnidadMedida,
      cantidad: this.count, // Usar la cantidad actual
    };

    // Agregar el producto al carrito a través del servicio
    this.cotizacionService.addToCart(product);

    // Obtener el carrito actualizado
    this.ListCarrito = this.cotizacionService.getCartItems();
  }
}
