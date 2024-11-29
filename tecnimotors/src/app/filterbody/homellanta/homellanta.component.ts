import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { MaestroarticuloService } from '../../core/service/maestroarticulo.service';
import Swiper from 'swiper';
import { Navigation, Pagination, Scrollbar } from 'swiper/modules';

@Component({
  selector: 'app-homellanta',
  templateUrl: './homellanta.component.html',
  styleUrls: ['./homellanta.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class HomellantaComponent implements OnInit, OnDestroy, AfterViewInit {
  /*
  ngAfterViewInit() {
    const swiper = new Swiper('.product__swiper--activation', {
      slidesPerView: 1,
      centeredSlides: false,
      slidesPerGroupSkip: 1,
      grabCursor: true,
      keyboard: {
        enabled: true,
      },
      breakpoints: {
        769: {
          slidesPerView: 4,
          slidesPerGroup: 4,
        },
      },
      scrollbar: {
        el: '.swiper-scrollbar',
      },
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      pagination: {
        el: '.swiper-pagination',
        clickable: true,
      },
    });
  }
  */

  // Import Swiper and modules
  public swiper: any;

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
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      scrollbar: {
        el: '.swiper-scrollbar',
        hide: true,
      },
      pagination: {
        el: '.swiper-pagination',
        type: 'fraction',
        clickable: true,
      },
    });
  }

  public titleGeneral: string = 'General';
  public titlellanta: string = 'General';
  public listaro: any[] = [];
  public listcocada: any[] = [];
  public listmarca: any[] = [];
  public listtipouso: any[] = [];
  public listarticulo: any[] = [];

  public txtaro: string = '';
  public txtcocada: string = '';
  public txtmarca: string = '';
  public txttipouso: string = '';

  public loading = false;
  public success = false;

  public limit = 2000;
  public offset = 0;
  public listinicio: any[] = [];
  public txtanchoperfil: string = '';
  public loadingperfilancho = false;

  public listasPorMarca: any[] = [];

  public defaultImage: string =
    '../../../assets/img/product/main-product/product1.webp';

  constructor(private servicesmaestro: MaestroarticuloService) {}

  public imagenError(event: any) {
    let ruta = this.defaultImage;
    event.target.src = ruta;
  }

  ngOnInit() {
    this.ListadoArticulo();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ListadoArticulo() {
    if (this.loadingperfilancho) return;
    this.loadingperfilancho = true;

    this.servicesmaestro.getMaestroArticuloAsync().subscribe({
      next: (dtl: any[]) => {
        this.listinicio = dtl;
        this.loadingperfilancho = false;
      },
      error: () => {
        this.loadingperfilancho = false;
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

  select(event: Event): void {
    const selectElement = event.target as HTMLSelectElement; // Obtiene el elemento select
    const selectedValue =
      selectElement.options[selectElement.selectedIndex].text; // Obtiene el texto de la opción seleccionada

    // Verifica si se seleccionó "Marca"
    this.titlellanta = selectedValue === 'Marca' ? 'General' : selectedValue;
  }

  SelectAnchoPerfil() {
    var anpe = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
    var ancho = anpe.split(' ')[0] == '--' ? '' : anpe.split(' ')[0];
    var perfil = anpe.split(' ')[1];
    var cocada = this.txtcocada == null ? '' : this.txtcocada;
    var aro = this.txtaro == null ? '' : this.txtaro;
    var marca = this.txtmarca == null ? '' : this.txtmarca;
    var tipouso = this.txttipouso == null ? '' : this.txttipouso;

    if (anpe != '') {
      this.loading = true;
      this.success = false;
      const frombody = {
        ancho: ancho,
        perfil: perfil,
        aro: aro,
        cocada: cocada,
        marca: marca,
        tipoUso: tipouso,
      };
      this.servicesmaestro.getAllfiltroPrincipalCocada(frombody).subscribe({
        next: ({
          listaro,
          listcocada,
          listmarca,
          lisTtipouso,
          listArticulo,
        }: any) => {
          setTimeout(() => {
            this.loading = false;
            this.success = true;
            this.listaro = listaro;
            this.listcocada = listcocada;
            this.listmarca = listmarca;
            this.listtipouso = lisTtipouso;
            this.listarticulo = listArticulo;
            if (this.swiper) {
              this.swiper.slideTo(0);
            }
            setTimeout(() => {
              this.success = false;
            }, 1000);
          }, 1000);
        },
        error: (err) => {
          console.error('Error al obtener datos:', err);
          this.loading = false; // Cambia el estado de carga en caso de error
        },
      });
    } else {
      this.loading = true;
      this.success = false;

      setTimeout(() => {
        this.loading = false;
        this.success = true;
        this.listaro = [];
        this.listcocada = [];
        this.listmarca = [];
        this.listtipouso = [];
        this.listarticulo = [];
        this.txtanchoperfil = '';
        this.txtaro = '';
        this.txtcocada = '';
        this.txtmarca = '';
        this.txttipouso = '';
        this.ListadoArticulo();
        setTimeout(() => {
          this.success = false;
        }, 1000);
      }, 1000);
    }
  }

  SelectAro() {
    var anpe = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
    var ancho = anpe.split(' ')[0] == '--' ? '' : anpe.split(' ')[0];
    var perfil = anpe.split(' ')[1];
    var cocada = this.txtcocada == null ? '' : this.txtcocada;
    var aro = this.txtaro == null ? '' : this.txtaro;
    var marca = this.txtmarca == null ? '' : this.txtmarca;
    var tipouso = this.txttipouso == null ? '' : this.txttipouso;

    this.loading = true;
    this.success = false;
    const frombody = {
      ancho: ancho,
      perfil: perfil,
      aro: aro,
      cocada: cocada,
      marca: marca,
      tipoUso: tipouso,
    };
    this.servicesmaestro.getAllfiltroPrincipalCocada(frombody).subscribe({
      next: ({
        listaro,
        listcocada,
        listmarca,
        lisTtipouso,
        listArticulo,
      }: any) => {
        setTimeout(() => {
          this.loading = false;
          this.success = true;
          this.listaro = listaro;
          this.listcocada = listcocada;
          this.listmarca = listmarca;
          this.listtipouso = lisTtipouso;
          this.listarticulo = listArticulo;
          if (this.swiper) {
            this.swiper.slideTo(0);
          }
          setTimeout(() => {
            this.success = false;
          }, 1000);
        }, 1000);
      },
      error: (err) => {
        console.error('Error al obtener datos:', err);
        this.loading = false; // Cambia el estado de carga en caso de error
      },
    });
  }

  selectCocada() {
    var anpe = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
    var ancho = anpe.split(' ')[0] == '--' ? '' : anpe.split(' ')[0];
    var perfil = anpe.split(' ')[1];
    var cocada = this.txtcocada == null ? '' : this.txtcocada;
    var aro = this.txtaro == null ? '' : this.txtaro;
    var marca = this.txtmarca == null ? '' : this.txtmarca;
    var tipouso = this.txttipouso == null ? '' : this.txttipouso;

    this.loading = true;
    this.success = false;
    const frombody = {
      ancho: ancho,
      perfil: perfil,
      aro: aro,
      cocada: cocada,
      marca: marca,
      tipoUso: tipouso,
    };
    this.servicesmaestro.getAllfiltroPrincipalCocada(frombody).subscribe({
      next: ({
        listaro,
        listcocada,
        listmarca,
        lisTtipouso,
        listArticulo,
      }: any) => {
        setTimeout(() => {
          this.loading = false;
          this.success = true;
          this.listaro = listaro;
          this.listcocada = listcocada;
          this.listmarca = listmarca;
          this.listtipouso = lisTtipouso;
          this.listarticulo = listArticulo;
          if (this.swiper) {
            this.swiper.slideTo(0);
          }
          setTimeout(() => {
            this.success = false;
          }, 1000);
        }, 1000);
      },
      error: (err) => {
        console.error('Error al obtener datos:', err);
        this.loading = false; // Cambia el estado de carga en caso de error
      },
    });
  }

  selectMarca() {
    var anpe = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
    var ancho = anpe.split(' ')[0] == '--' ? '' : anpe.split(' ')[0];
    var perfil = anpe.split(' ')[1];
    var cocada = this.txtcocada == null ? '' : this.txtcocada;
    var aro = this.txtaro == null ? '' : this.txtaro;
    var marca = this.txtmarca == null ? '' : this.txtmarca;
    var tipouso = this.txttipouso == null ? '' : this.txttipouso;

    this.loading = true;
    this.success = false;
    const frombody = {
      ancho: ancho,
      perfil: perfil,
      aro: aro,
      cocada: cocada,
      marca: marca,
      tipoUso: tipouso,
    };
    this.servicesmaestro.getAllfiltroPrincipalCocada(frombody).subscribe({
      next: ({
        listaro,
        listcocada,
        listmarca,
        lisTtipouso,
        listArticulo,
      }: any) => {
        setTimeout(() => {
          this.loading = false;
          this.success = true;
          this.listaro = listaro;
          this.listcocada = listcocada;
          this.listmarca = listmarca;
          this.listtipouso = lisTtipouso;
          this.listarticulo = listArticulo;
          if (this.swiper) {
            this.swiper.slideTo(0);
          }
          setTimeout(() => {
            this.success = false;
          }, 1000);
        }, 1000);
      },
      error: (err) => {
        console.error('Error al obtener datos:', err);
        this.loading = false; // Cambia el estado de carga en caso de error
      },
    });
  }

  selectTipoUso() {
    var anpe = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
    var ancho = anpe.split(' ')[0] == '--' ? '' : anpe.split(' ')[0];
    var perfil = anpe.split(' ')[1];
    var cocada = this.txtcocada == null ? '' : this.txtcocada;
    var aro = this.txtaro == null ? '' : this.txtaro;
    var marca = this.txtmarca == null ? '' : this.txtmarca;
    var tipouso = this.txttipouso == null ? '' : this.txttipouso;

    this.loading = true;
    this.success = false;
    const frombody = {
      ancho: ancho,
      perfil: perfil,
      aro: aro,
      cocada: cocada,
      marca: marca,
      tipoUso: tipouso,
    };
    this.servicesmaestro.getAllfiltroPrincipalCocada(frombody).subscribe({
      next: ({
        listaro,
        listcocada,
        listmarca,
        lisTtipouso,
        listArticulo,
      }: any) => {
        setTimeout(() => {
          this.loading = false;
          this.success = true;
          this.listaro = listaro;
          this.listcocada = listcocada;
          this.listmarca = listmarca;
          this.listtipouso = lisTtipouso;
          this.listarticulo = listArticulo;

          if (this.swiper) {
            this.swiper.slideTo(0);
          }
          setTimeout(() => {
            this.success = false;
          }, 1000);
        }, 1000);
      },
      error: (err) => {
        console.error('Error al obtener datos:', err);
        this.loading = false; // Cambia el estado de carga en caso de error
      },
    });
  }

  Limpiar() {
    this.listaro = [];
    this.listcocada = [];
    this.listmarca = [];
    this.listtipouso = [];
    this.listarticulo = [];
    this.txtanchoperfil = '';
    this.txtaro = '';
    this.txtcocada = '';
    this.txtmarca = '';
    this.txttipouso = '';
  }
}
