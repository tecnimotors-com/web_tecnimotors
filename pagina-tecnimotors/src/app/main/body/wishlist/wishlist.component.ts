import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { SharedMain } from '../../sharedmain';
import { AuthService } from '../../../core/service/auth.service';
import { MarketingService } from '../../../core/service/marketing.service';
import { CotizacionService } from '../../../core/service/cotizacion.service';
import { MinoristaService } from '../../../core/service/marketing/minorista.service';
import { DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment.development';

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
  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public srcimg: string = 'assets/img/product/main-product/product6.webp';
  private cartSubscription!: Subscription | undefined; // Para manejar la suscripción
  public ListCarrito: any[] = [];

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
      }
    );
    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items; // Actualiza la lista de productos en el carrito
    });

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
  eliminarwishlist(item: any) {
    this.cotizacionService.eliminarProductoWish(item);
    this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
      (items) => {
        this.wishlistItems = items; // Actualiza la lista de productos en el carrito
      }
    );
  }
  AgregarCarrito(item: any) {
    const product = {
      descripcion: item.descripcion,
      unidad: item.unidadmedida,
      categoria: item.aplicacion,
      marca: item.marca,
      marcaoriginal: item.marcaoriginal,
      medida: item.medida,
      modelo: item.modelo,
      medidaestandarizado: item.medidaestandarizado,
      id: item.id,
      codigo: item.codigo,
      familia: item.familia,
      subfamilia: item.subfamilia,
      tipo: item.tipo,
      cantidad: 1,
      sku: item.codigo,
      producto: item.marcaoriginal,
      vendor: item.marca,
      quantity: item.cantidad,
      color: '',
      pathimagen: item.pathimagen,
    };
    this.cotizacionService.addToCart2(product);
    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items; // Actualiza la lista de productos en el carrito
    });
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
