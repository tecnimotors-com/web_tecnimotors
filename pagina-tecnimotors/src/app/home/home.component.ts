import {
  Component,
  AfterViewInit,
  OnInit,
  OnDestroy,
  ElementRef,
  ViewChild,
  HostListener,
} from '@angular/core';
import { Router } from '@angular/router';
import Swiper from 'swiper';
import { Navigation, Pagination, Scrollbar } from 'swiper/modules';
import { AuthService } from '../core/service/auth.service';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaestroclasificadoService } from '../core/service/maestroclasificado.service';
import { PreloaderComponent } from '../main/helper/preloader/preloader.component';

@Component({
  selector: 'app-home',
  imports: [
    HeaderComponent,
    FooterComponent,
    CommonModule,
    FormsModule,
    PreloaderComponent,
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy, AfterViewInit {
  constructor(
    private router: Router,
    private auth: AuthService,
    private maestroclasifi: MaestroclasificadoService
  ) {}
  @ViewChild('swiper', { static: false }) swiperContainer!: ElementRef;
  @ViewChild('swiperContainer', { static: false })
  swiperContainer2!: ElementRef;
  public swiper: any;
  public ListProducto: any[] = [
    {
      producthome: 'Llantas',
      idproduct: 1,
    },
    {
      producthome: 'Cámaras',
      idproduct: 2,
    },
    {
      producthome: 'Repuestos',
      idproduct: 3,
    },
    {
      producthome: 'Aceites y Lubricantes',
      idproduct: 4,
    },
    {
      producthome: 'Vehiculos',
      idproduct: 5,
    },
  ];

  public ListTipoVehiculo: any[] = [];
  public istruevehiculo: boolean = false;
  public istruetipovehiculo: boolean = false;
  public selectedTipoVehiculo: string = '';
  public txtproduchome: string = '';
  public ListMarca: any[] = [];

  public txtSelectmarca: string = '';
  public selectid: string = '0';
  public selecttipo: string = '';
  public selectmarca: string = '';

  ngAfterViewInit() {
    this.swiper = new Swiper(this.swiperContainer2.nativeElement, {
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
    /*
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
    */
  }

  logos = [
    {
      src: 'assets/img/logo/logotecnimotors/RTM.jpg',
      route: '/homellantas',
    },
    {
      src: 'assets/img/logo/logotecnimotors/TSK.jpg',
      route: '/homellantas',
    },
    {
      src: 'assets/img/logo/logotecnimotors/KENDA.jpg',
      route: '/homellantas',
    },
    {
      src: 'assets/img/logo/logotecnimotors/DUNLOP.jpg',
      route: '/homellantas',
    },
    {
      src: 'assets/img/logo/logotecnimotors/CHENG-SHIN.jpg',
      route: '/homellantas',
    },
    {
      src: 'assets/img/logo/logotecnimotors/MAXXIS.jpg',
      route: '/homellantas',
    },
    {
      src: 'assets/img/logo/logotecnimotors/WANDA.jpg',
      route: '/homellantas',
    },
  ];

  public listarticulo: any[] = [
    { src: '1.jpg' },
    { src: '1_1.jpg' },
    { src: 'PORTADAS-PAGINA-WEB.jpg' },
  ];
  public src1: string = 'assets/img/banner/tecnimotors/1.jpg';
  public src2: string = 'assets/img/banner/tecnimotors/1_1.jpg';
  public src3: string = 'assets/img/banner/tecnimotors/PORTADAS-PAGINA-WEB.jpg';
  public isVisible: boolean = false;

  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Verifica si el scroll es mayor a 200px
    this.isVisible = window.scrollY > 200;
  }

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ngOnInit(): void {
    this.auth.getRefreshToken();
  }

  ngOnDestroy(): void {}

  selectProducto(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    let selectedId = selectElement.value;
    this.selectid = selectedId ?? '0';

    if (selectedId == '1') {
      this.resetSelect();
      this.ListTipoVehiculo = [
        {
          productipovehiculo: 'Bicicletas',
          idproducttipo: 1,
        },
        {
          productipovehiculo: 'Motocicletas',
          idproducttipo: 2,
        },
      ];
      //selectedId = '10';
      this.istruevehiculo = true;
      this.istruetipovehiculo = true;
    } else if (selectedId == '2') {
      this.resetSelect();
      this.ListTipoVehiculo = [
        {
          productipovehiculo: 'Bicicletas',
          idproducttipo: 1,
        },
        {
          productipovehiculo: 'Motocicletas',
          idproducttipo: 2,
        },
      ];
      this.istruevehiculo = true;
      this.istruetipovehiculo = true;
    } else if (selectedId == '3') {
      this.resetSelect();
      this.ListTipoVehiculo = [
        {
          productipovehiculo: 'Bicicletas',
          idproducttipo: 1,
        },
        {
          productipovehiculo: 'Motocicletas',
          idproducttipo: 2,
        },
      ];
      this.istruevehiculo = true;
      this.istruetipovehiculo = true;
    } else if (selectedId == '4') {
      this.resetSelect();
      this.ListTipoVehiculo = [
        {
          productipovehiculo: 'Motocicletas',
          idproducttipo: 2,
        },
      ];
      this.istruevehiculo = true;
      this.istruetipovehiculo = true;
      this.selectedTipoVehiculo = '2';
    } else {
      this.resetSelect();

      this.maestroclasifi.getListadoCategoriaVehiculos().subscribe({
        next: (lst: any) => {
          this.ListTipoVehiculo = lst;
          this.istruevehiculo = false;
          this.istruetipovehiculo = true;
        },
      });
    }
  }
  resetSelect(): void {
    this.selectedTipoVehiculo = ''; // Restablece el valor a null
  }
  selectTipoProducto(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedId = selectElement.value;
    this.selecttipo = this.selectedTipoVehiculo;
    if (this.selecttipo == '1') {
      this.ListMarca = [
        {
          productmarca: 'DUNLOP',
        },
        {
          productmarca: 'KENDA',
        },
        {
          productmarca: 'CELIMO',
        },
        {
          productmarca: 'CS',
        },
      ];
      this.txtSelectmarca = '';
    } else if (this.selecttipo == '2') {
      this.ListMarca = [
        {
          productmarca: 'DUNLOP',
        },
        {
          productmarca: 'KENDA',
        },
        {
          productmarca: 'CELIMO',
        },
        {
          productmarca: 'CS',
        },
      ];
      this.txtSelectmarca = '';
    } else {
    }
  }
  selectMarca() {
    this.selectmarca = this.txtSelectmarca;
  }

  BtnBuscar() {
    const sectmarca = this.txtSelectmarca == '' ? '0' : this.txtSelectmarca;
    this.auth.navigate(this.selectid, this.selecttipo, sectmarca);
  }

  BtnVehiculoMotocicleta() {
    localStorage.setItem('categoriavehiculo', 'Motocicleta');
    this.router.navigate(['/homevehiculo/Motocicleta']);
  }
  BtnVehiculoBicicleta() {
    localStorage.setItem('categoriavehiculo', 'Bicicleta');
    this.router.navigate(['/homevehiculo/Bicicleta']);
  }
  BtnVehiculoCuatrimotos() {
    localStorage.setItem('categoriavehiculo', 'Cuatrimoto');
    this.router.navigate(['/homevehiculo/Cuatrimoto']);
  }
  BtnVehiculoCargueros() {
    localStorage.setItem('categoriavehiculo', 'Trimoto de carga');
    this.router.navigate(['/homevehiculo/Trimoto de carga']);
  }

  BtnLlantas() {
    this.router.navigate(['/homellantas']);
  }

  BtnCamara() {
    this.router.navigate(['/homecamara']);
  }

  BtnAceite() {
    this.router.navigate(['/homeaceite']);
  }

  BtnRepuesto() {
    this.router.navigate(['/homerepuesto']);
  }
}
