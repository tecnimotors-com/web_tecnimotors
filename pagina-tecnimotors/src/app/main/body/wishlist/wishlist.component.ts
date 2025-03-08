import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { SharedMain } from '../../sharedmain';
import { AuthService } from '../../../core/service/auth.service';
import { MarketingService } from '../../../core/service/marketing.service';
import { CotizacionService } from '../../../core/service/cotizacion.service';
import { MinoristaService } from '../../../core/service/marketing/minorista.service';
import { DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import Swal from 'sweetalert2';
import CryptoJS from 'crypto-js';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-wishlist',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.scss'],
})
export class WishlistComponent implements OnInit, OnDestroy {
  public isVisible: boolean = false;
  public isAuthenticated: boolean = false;
  public isProcessingCarrito: boolean = false;
  public ListWishlist: any[] = [];
  public wishlistItems: any[] = [];
  public ListCarrito: any[] = [];
  public cartItems: any[] = [];

  public currentUrl: string = '';
  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public srcimg: string = 'assets/img/product/main-product/product6.webp';

  private cartSubscription!: Subscription | undefined;
  private wishlistSubscription!: Subscription | undefined;

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
    private datePipe: DatePipe,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();
    // Capturar la URL completa
    this.currentUrl = this.router.url;
    this.auth.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (this.isAuthenticated) {
        const txtuuid = this.decrypt(localStorage.getItem('uuid')!);
        this.loadWishlist(txtuuid); // Cargar la wishlist desde la base de datos
      } else {
        // Suscribirse a los cambios en la wishlist
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.wishlistItems = items;
          }
        );
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.ListCarrito = items; // Actualiza la lista de productos en el carrito
          }
        );
      }
    });
  }

  private decrypt(data: string): string {
    if (!data) {
      return '';
    }
    const bytes = CryptoJS.AES.decrypt(data, environment.apikeencriptado);
    return bytes.toString(CryptoJS.enc.Utf8);
  }

  private loadWishlist(txtuuid: string): void {
    this.cotizacionService.getAllWishlist(txtuuid).subscribe((dtl: any) => {
      this.wishlistItems = dtl;
    });
    this.cotizacionService.getAllCarritoList(txtuuid).subscribe((dtl: any) => {
      this.cartItems = dtl;
    });
  }

  isInCarritolist(productIdentifier: string): boolean {
    if (this.isAuthenticated) {
      return this.cartItems.some(
        (item) => item.codigo.trim() === productIdentifier.trim()
      );
    } else {
      return this.cartItems.some((item) => item.codigo === productIdentifier);
    }
  }
  private showAlert(texto: string, type: any): void {
    Swal.fire({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 1500,
      timerProgressBar: true,
      title: texto,
      icon: type,
    });
  }

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ngOnDestroy(): void {
    this.wishlistSubscription?.unsubscribe();
  }

  eliminarwishlist(item: any) {
    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      this.cotizacionService
        .removeFromWishlist(txtuuid, item.codigo.trim())
        .subscribe({
          next: () => {
            this.loadWishlist(txtuuid);
            this.showAlert(
              'Producto eliminado de la lista de deseos',
              'success'
            );
          },
          error: () => {
            this.showAlert('Error al eliminar de la lista de deseos', 'error');
          },
        });
    } else {
      this.cotizacionService.eliminarProductoWish(item);
      this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
        (items) => {
          this.wishlistItems = items; // Actualiza la lista de productos en el carrito
        }
      );
    }
  }

  AgregarCarrito(item: any) {
    if (this.isProcessingCarrito) return;
    this.isProcessingCarrito = true;
    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      if (this.isInCarritolist(item.codigo)) {
        this.cotizacionService
          .RemoveFromCarritoList(txtuuid, item.codigo.trim())
          .subscribe({
            next: () => {
              this.loadWishlist(txtuuid); // Cargar la wishlist actualizada
              this.showAlert(
                'Producto eliminado de la lista de cotización',
                'success'
              );
              this.isProcessingCarrito = false;
            },
            error: () => {
              this.isProcessingCarrito = false;
              this.showAlert(
                'Error al eliminar a la lista de cotización',
                'error'
              );
            },
          });
      } else {
        const authCarrito = this.createAuthCarrito(item);
        this.cotizacionService.getRegisterCarritoList(authCarrito).subscribe({
          next: () => {
            this.loadWishlist(txtuuid); // Actualiza la wishlist después de agregar
            this.showAlert(
              'Producto agregado a la lista de cotización',
              'success'
            );
            this.isProcessingCarrito = false;
          },
          error: () => {
            this.isProcessingCarrito = false;
            this.showAlert(
              'Error al agregar a la lista de cotización',
              'error'
            );
          },
        });
      }
    } else {
      const product = {
        descripcion: item.descripcion,
        unidad: item.unidad,
        categoria: item.categoria,
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
        quantity: 1,
        color: '',
        pathimagen: item.pathimagen,
      };
      if (this.isInCarritolist(item.codigo)) {
        this.cotizacionService.eliminarProducto(item);
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.isProcessingCarrito = false;
            this.ListCarrito = items; // Actualiza la lista de productos en el carrito
          }
        );
      } else {
        this.cotizacionService.addToCart2(product);
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.isProcessingCarrito = false;
            this.ListCarrito = items; // Actualiza la lista de productos en el carrito
          }
        );
      }
    }
  }
  private createAuthCarrito(item: any): any {
    console.log(item);
    return {
      uuidcliente: this.decrypt(localStorage.getItem('uuid')!),
      ordernshopping: '',
      descripcion: item.descripcion,
      unidad: item.unidad,
      categoria: item.categoria,
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
      quantity: 1,
      color: '',
      pathimagen: item.pathimagen,
      estado: 'Activo',
      fecharegistro: new Date().toISOString(),
    };
  }
}
