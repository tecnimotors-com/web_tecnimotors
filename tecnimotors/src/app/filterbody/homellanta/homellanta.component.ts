import { Component, OnDestroy, OnInit, AfterViewInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';

@Component({
  selector: 'app-homellanta',
  templateUrl: './homellanta.component.html',
  styleUrls: ['./homellanta.component.css'],
})
export class HomellantaComponent implements OnInit, OnDestroy {
  public titlellanta: string = 'General';

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

  select(event: Event): void {
    const selectElement = event.target as HTMLSelectElement; // Obtiene el elemento select
    const selectedValue =
      selectElement.options[selectElement.selectedIndex].text; // Obtiene el texto de la opción seleccionada

    // Verifica si se seleccionó "Marca"
    this.titlellanta = selectedValue === 'Marca' ? 'General' : selectedValue;
  }
}
