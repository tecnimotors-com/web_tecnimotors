import { Component, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import Swiper from 'swiper';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
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
