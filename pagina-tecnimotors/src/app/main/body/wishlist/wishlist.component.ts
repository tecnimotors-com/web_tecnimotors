import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { SharedMain } from '../../sharedmain';
import { AuthService } from '../../../core/service/auth.service';
import { MarketingService } from '../../../core/service/marketing.service';
import { CotizacionService } from '../../../core/service/cotizacion.service';
import { MinoristaService } from '../../../core/service/marketing/minorista.service';
import { DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-wishlist',
  imports: [SharedMain],
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.scss'],
})
export class WishlistComponent implements OnInit, OnDestroy {
  public isVisible: boolean = false;
  private wishlistSubscription!: Subscription | undefined;
  public ListWishlist: any[] = [];
  public wishlistItems: any[] = [];

  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Verifica si el scroll es mayor a 200px
    this.isVisible = window.scrollY > 200;
  }

  constructor(
    private auth: AuthService,
    private marketing: MarketingService,
    private cotizacionService: CotizacionService,
    private minoristaservice: MinoristaService,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();
    this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
      (items) => {
        this.wishlistItems = items;
        console.log(items);
      }
    );
    this.initializePreLoader();
    setTimeout(() => {
      this.finalizePreLoader();
    }, 1000);
  }

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  private initializePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');

    if (preloaderWrapper) {
      preloaderWrapper.classList.remove('loaded');
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
