import { Component, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import Swiper from 'swiper';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  logos = [
    {
      src: '../../assets/img/logo/logotecnimotors/RTM.jpg',
      route: '/homellantas',
    },
    {
      src: '../../assets/img/logo/logotecnimotors/TSK.jpg',
      route: '/homellantas',
    },
    {
      src: '../../assets/img/logo/logotecnimotors/KENDA.jpg',
      route: '/homellantas',
    },
    {
      src: '../../assets/img/logo/logotecnimotors/DUNLOP.jpg',
      route: '/homellantas',
    },
    {
      src: '../../assets/img/logo/logotecnimotors/CHENG-SHIN.jpg',
      route: '/homellantas',
    },
    {
      src: '../../assets/img/logo/logotecnimotors/MAXXIS.jpg',
      route: '/homellantas',
    },
    {
      src: '../../assets/img/logo/logotecnimotors/WANDA.jpg',
      route: '/homellantas',
    },
  ];  
  /*
  public src1: string =
  '../../assets/img/banner/prueba/LLANTAS.jpg';
  public src2: string =
  '../../assets/img/banner/prueba/MOTOCICLETA.jpg';
  public src3: string =
  '../../assets/img/banner/prueba/CUATRIMOTO.jpg';
  */
  
  public src1: string =
    '../../assets/img/banner/tecnimotors/PORTADAS-PAGINA-WEB-5.jpg';
  public src2: string =
    '../../assets/img/banner/tecnimotors/PORTADAS-PAGINA-WEB-6.jpg';
  public src3: string =
    '../../assets/img/banner/tecnimotors/PORTADAS-PAGINA-WEB.jpg';
  
  //OnInit, OnDestroy,
  //swiper!: Swiper;
  //public swiper?: Swiper;

  ngOnInit(): void {
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
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
