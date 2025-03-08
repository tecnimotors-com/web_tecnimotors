import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { SharedMain } from '../../../sharedmain';
import { MaestroarticuloService } from '../../../../core/service/maestroarticulo.service';
import { CotizacionService } from '../../../../core/service/cotizacion.service';
import { AuthService } from '../../../../core/service/auth.service';
import { MaestroclasificadoService } from '../../../../core/service/maestroclasificado.service';
import { environment } from '../../../../../environments/environment.development';
import CryptoJS from 'crypto-js';
import Swal from 'sweetalert2';
import { PreloaderComponent } from '../../../helper/preloader/preloader.component';
import { Meta, Title } from '@angular/platform-browser';

@Component({
  selector: 'app-detallevehiculo',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './detallevehiculo.component.html',
  styleUrls: ['./detallevehiculo.component.scss'],
})
export class DetallevehiculoComponent implements OnInit, OnDestroy {
  public srcimg: string =
    '../../../../assets/img/product/big-product/product1.webp';
  public lbldescripcion: string = '';
  public lblunidadmedida: string = '';
  public lblcategoria: string = '';
  public lblmarca: string = '';
  public lblmarcaoriginal: string = '';
  public lblaplicacion: string = '';
  public lblproducto: string = '';
  public lblmedida: string = '';
  public lblmodelo: string = '';
  public lblmedidaestandarizado: string = '';
  public lbltipo1: string = '';
  public lblid: number = 0;
  public lblcodigo: string = '';
  public lblfamilia: string = '';
  public lblsubfamilia: string = '';
  public lbltipo: string = '';
  public lblcodigoequivalente: string = '';
  public lblpathoriginal: string = '';
  public lblpathimagen: string = '';
  public lbltotalImagenes: number = 0;

  public count: number = 1;
  public blndisable = false;
  public txtacuerdotrue: boolean = false;
  public isVisible: boolean = false;

  private cartSubscription!: Subscription | undefined;
  private wishlistSubscription!: Subscription | undefined;

  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public txtlinkoriginal: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';

  public ListCarrito: any[] = [];
  public Listrutaoriginal: any[] = [];
  public ListWishlist: any[] = [];
  public wishlistItems: any[] = [];
  public cartItems: any[] = [];

  public isAuthenticated: boolean = false;
  public isProcessing: boolean = false;
  public isProcessingCarrito: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private auth: AuthService,
    private cotizacionService: CotizacionService,
    private serviceclasificado: MaestroclasificadoService,
    private meta: Meta,
    private title: Title
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshTokenvehiculo();
    this.DetalleVehiculo();

    this.auth.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (this.isAuthenticated) {
        const txtuuid = this.decrypt(localStorage.getItem('uuid')!);
        this.loadWishlist(txtuuid); // Cargar la wishlist desde la base de datos
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.ListCarrito = items;
          }
        );
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.wishlistItems = items;
          }
        );
      } else {
        // Suscribirse a los cambios en la wishlist
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.wishlistItems = items;
          }
        );
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.ListCarrito = items;
          }
        );
        this.ListCarrito = this.cotizacionService.getCartItems();
      }
    });
  }

  ngOnDestroy(): void {
    this.wishlistSubscription?.unsubscribe();
  }

  DetalleVehiculo() {
    this.route.params.subscribe((params) => {
      this.lblid = parseInt(params['id']);
      this.serviceclasificado.getDetalleVehiculo(this.lblid).subscribe({
        next: (dtl: any) => {
          this.lbldescripcion = dtl.listado.descripcion;
          this.lblunidadmedida = dtl.listado.unidadmedida;
          this.lblcategoria = dtl.listado.categoria;
          this.lblmarca = dtl.listado.marca;
          this.lblmarcaoriginal = dtl.listado.marcaoriginal;
          this.lblmedida = dtl.listado.medida;
          this.lblmodelo = dtl.listado.modelo;
          this.lblmedidaestandarizado = dtl.listado.medidaestandarizado;
          this.lblid = dtl.listado.id;
          this.lblcodigo = dtl.listado.codigo;
          this.lblfamilia = dtl.listado.familia;
          this.lblsubfamilia = dtl.listado.subfamilia;
          this.lbltipo = dtl.listado.tipo;

          this.lblcodigoequivalente = dtl.listado.codigoequivalente;
          this.lblaplicacion = dtl.listado.aplicacion;
          this.lblproducto = dtl.listado.producto;
          this.lbltipo1 = dtl.listado.tipo1;

          this.lblpathoriginal = dtl.primeraRutaOriginal;
          this.Listrutaoriginal = dtl.rutasOriginales;
          this.lbltotalImagenes = dtl.totalImagenes;
          this.lblpathimagen = dtl.pathimagen;
          // Establecer los metadatos
          this.setMetaData();
        },
      });
    });
  }

  setMetaData() {
    // Establecer el título de la página
    this.title.setTitle(
      `${this.lblcodigo.trim().slice(-5)} - ${this.lblcodigoequivalente}`
    );
    //{{lblcodigo.trim().slice(-5)}} - {{lblcodigoequivalente}}
    // Establecer la descripción
    this.meta.updateTag({ name: 'description', content: this.lbldescripcion });

    // Establecer palabras clave (opcional)
    this.meta.updateTag({
      name: 'keywords',
      content: `${this.lblmarcaoriginal},${this.lbldescripcion}, ${
        this.lblcodigoequivalente
      }, ${this.lblcodigo.trim().slice(-5)}`,
    });

    // Establecer otras etiquetas meta según sea necesario
    this.meta.updateTag({
      property: 'og:title',
      content: this.lblcodigo.trim().slice(-5) + this.lblcodigoequivalente,
    });
    this.meta.updateTag({
      property: 'og:description',
      content: this.lbldescripcion,
    });
    this.meta.updateTag({
      property: 'og:image',
      content: this.txtlink + this.lblpathoriginal,
    }); // Asegúrate de que esta URL sea accesible
    this.meta.updateTag({
      property: 'og:url',
      content: this.txtlink + this.lblpathoriginal,
    });
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
    if (this.isProcessingCarrito) return;
    this.isProcessingCarrito = true;
    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      if (this.isInCarritolist(this.lblcodigo)) {
        this.cotizacionService
          .UpdateCantidadCarritoList(txtuuid, this.lblcodigo.trim(), this.count)
          .subscribe({
            next: () => {
              this.loadWishlist(txtuuid); // Cargar la wishlist actualizada
              this.showAlert('Cantidad Actualizado', 'success');
              this.isProcessingCarrito = false;
            },
            error: () => {
              this.isProcessingCarrito = false;
              this.showAlert('Error al actualizar la cantidad', 'error');
            },
          });
      } else {
        const authCarrito = this.createAuthCarritoModal();
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
        descripcion: this.lbldescripcion,
        unidad: this.lblunidadmedida,
        categoria: this.lblcategoria,
        marca: this.lblmarca,
        marcaoriginal: this.lblmarcaoriginal,
        medida: this.lblmedida,
        modelo: this.lblmodelo,
        medidaestandarizado: this.lblmedidaestandarizado,
        id: this.lblid,
        codigo: this.lblcodigo,
        familia: this.lblfamilia,
        subfamilia: this.lblsubfamilia,
        tipo: this.lbltipo,
        cantidad: this.count ?? 1,

        sku: this.lblcodigo,
        producto: this.lblmarcaoriginal,
        vendor: this.lblmarca,
        quantity: this.count,
        color: '',
        pathimagen: this.lblpathoriginal,
      };
      this.cotizacionService.addToCart2(product);
      this.cartSubscription = this.cotizacionService.cart$.subscribe(
        (items) => {
          this.isProcessingCarrito = false;
          this.ListCarrito = items;
        }
      );
    }
  }

  DtlImagen1(item: string) {
    this.lblpathoriginal = item; // Cambia la imagen principal a la que se hizo clic
  }
  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  MdWishListFavorito() {
    if (this.isProcessing) return; // Evitar múltiples clics
    this.isProcessing = true; // Establecer el estado de procesamiento

    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      // Verificar si el producto ya está en la wishlist usando item.codigo
      if (this.isInWishlist(this.lblcodigo.trim())) {
        // Si ya está en la wishlist, marcarlo como inactivo
        this.cotizacionService
          .removeFromWishlist(txtuuid, this.lblcodigo.trim())
          .subscribe({
            next: () => {
              this.loadWishlist(txtuuid); // Cargar la wishlist actualizada
              this.showAlert(
                'Producto eliminado de la lista de deseos',
                'success'
              );
              this.isProcessing = false;
            },
            error: () => {
              this.isProcessing = false;
              this.showAlert(
                'Error al eliminar de la lista de deseos',
                'error'
              );
            },
          });
      } else {
        // Si no está en la wishlist, agregarlo
        this.cotizacionService
          .getNextOrderNumber2(txtuuid.toString())
          .subscribe({
            next: (dtl: any) => {
              const authProduct = this.createAuthProduct(dtl.orderNumber);
              this.cotizacionService
                .getRegisterWishList(authProduct)
                .subscribe({
                  next: () => {
                    this.loadWishlist(txtuuid); // Actualiza la wishlist después de agregar
                    this.showAlert(
                      'Producto agregado a la lista de deseos',
                      'success'
                    );
                    this.isProcessing = false;
                  },
                  error: () => {
                    this.isProcessing = false;
                    this.showAlert(
                      'Error al agregar a la lista de deseos',
                      'error'
                    );
                  },
                });
            },
          });
      }
    } else {
      const product = {
        descripcion: this.lbldescripcion,
        unidad: this.lblunidadmedida,
        categoria: this.lblcategoria,
        marca: this.lblmarca,
        marcaoriginal: this.lblmarcaoriginal,
        medida: this.lblmedida,
        modelo: this.lblmodelo,
        medidaestandarizado: this.lblmedidaestandarizado,
        id: this.lblid,
        codigo: this.lblcodigo,
        familia: this.lblfamilia,
        subfamilia: this.lblsubfamilia,
        tipo: this.lbltipo,
        cantidad: this.count ?? 1,
        sku: this.lblcodigo,
        producto: this.lblmarcaoriginal,
        vendor: this.lblmarca,
        quantity: this.count,
        color: '',
        pathimagen: this.lblpathoriginal,
      };
      this.cotizacionService.addToWishlist(product);
      this.wishlistSubscription = this.cotizacionService.cart$.subscribe(
        (items) => {
          this.isProcessing = false;
          this.ListWishlist = items; // Actualiza la lista de productos en el carrito
        }
      );
    }
  }

  imagenloading(item: any) {
    this.lblpathoriginal = item;
  }

  isInWishlist(productIdentifier: string): boolean {
    if (this.isAuthenticated) {
      // Si está autenticado, usa el método que verifica por código
      return this.wishlistItems.some(
        (item) => item.codigo.trim() === productIdentifier.trim()
      );
    } else {
      // Si no está autenticado, usa el método que verifica por id
      return this.wishlistItems.some(
        (item) => item.codigo === productIdentifier
      );
    }
  }

  private loadWishlist(txtuuid: string): void {
    this.cotizacionService.getAllWishlist(txtuuid).subscribe((dtl: any) => {
      this.wishlistItems = dtl;
    });
    this.cotizacionService.getAllCarritoList(txtuuid).subscribe((dtl: any) => {
      this.cartItems = dtl;
    });
  }

  private decrypt(data: string): string {
    if (!data) {
      return '';
    }
    const bytes = CryptoJS.AES.decrypt(data, environment.apikeencriptado);
    return bytes.toString(CryptoJS.enc.Utf8);
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

  private createAuthProduct(orderNumber: string): any {
    return {
      uuidcliente: this.decrypt(localStorage.getItem('uuid')!),
      ordenwishlist: orderNumber,
      descripcion: this.lbldescripcion,
      unidad: this.lblunidadmedida,
      categoria: this.lblcategoria,
      marca: this.lblmarca,
      marcaoriginal: this.lblmarcaoriginal,
      medida: this.lblmedida,
      modelo: this.lblmodelo,
      medidaestandarizado: this.lblmedidaestandarizado,
      id: this.lblid,
      codigo: this.lblcodigo.trim(),
      familia: this.lblfamilia,
      subfamilia: this.lblsubfamilia,
      tipo: this.lbltipo,
      cantidad: this.count ?? 1,
      sku: this.lblcodigo,
      producto: this.lblmarcaoriginal,
      vendor: this.lblmarca,
      quantity: this.count,
      color: '',
      pathimagen: this.lblpathimagen,
      estado: 'Activo',
      fecharegistro: new Date().toISOString(),
    };
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

  private createAuthCarritoModal() {
    return {
      uuidcliente: this.decrypt(localStorage.getItem('uuid')!),
      ordernshopping: '',
      descripcion: this.lbldescripcion,
      unidad: this.lblunidadmedida,
      categoria: this.lblcategoria,
      marca: this.lblmarca,
      marcaoriginal: this.lblmarcaoriginal,
      medida: this.lblmedida,
      modelo: this.lblmodelo,
      medidaestandarizado: this.lblmedidaestandarizado,
      id: this.lblid,
      codigo: this.lblcodigo,
      familia: this.lblfamilia,
      subfamilia: this.lblsubfamilia,
      tipo: this.lbltipo,
      cantidad: this.count ?? 1,

      sku: this.lblcodigo,
      producto: this.lblmarcaoriginal,
      vendor: this.lblmarca,
      quantity: this.count,
      color: '',
      pathimagen: this.lblpathimagen,
      estado: 'Activo',
      fecharegistro: new Date().toISOString(),
    };
  }
}
