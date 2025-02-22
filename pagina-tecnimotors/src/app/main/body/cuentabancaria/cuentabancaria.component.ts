import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { SharedMain } from '../../sharedmain';
import { listdata } from './cuentadata';
@Component({
  selector: 'app-cuentabancaria',
  imports: [SharedMain],
  templateUrl: './cuentabancaria.component.html',
  styleUrls: ['./cuentabancaria.component.scss'],
})
export class CuentabancariaComponent implements OnInit, OnDestroy {
  public Listbancario: any[] = listdata;

  constructor(private auth: AuthService) {}
  ngOnInit(): void {
    this.auth.getRefreshToken();
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
