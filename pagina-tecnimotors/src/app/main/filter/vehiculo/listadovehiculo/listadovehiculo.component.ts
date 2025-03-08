import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/service/auth.service';
import { SharedMain } from '../../../sharedmain';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroclasificadoService } from '../../../../core/service/maestroclasificado.service';
import Swal from 'sweetalert2';
import { CotizacionService } from '../../../../core/service/cotizacion.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { MinoristaService } from '../../../../core/service/marketing/minorista.service';
import { environment } from '../../../../../environments/environment.development';
import { PreloaderComponent } from '../../../helper/preloader/preloader.component';
import CryptoJS from 'crypto-js';

@Component({
  selector: 'app-listadovehiculo',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './listadovehiculo.component.html',
  styleUrls: ['./listadovehiculo.component.scss'],
})
export class ListadovehiculoComponent implements OnInit {
  public ListarCategoria: any[] = [];
  public ListModelo: any[] = [];
  public ListMarca: any[] = [];
  public ListGeneral: any[] = [];
  public ListCarrito: any[] = [];
  public ListWishlist: any[] = [];

  public wishlistItems: any[] = [];
  public cartItems: any[] = [];

  public txtcategoria: string = localStorage.getItem('categoriavehiculo') || '';
  public txtmodelo: string = localStorage.getItem('modelovehiculo') || '';
  public txtmarca: string = localStorage.getItem('marcavehiculo') || '';
  public p: number = parseInt(localStorage.getItem('pvehiculo')!) || 1;

  public count: number = 1;
  public itemper: number = 10;

  public isVisible: boolean = false;
  public blndisable: boolean = false;
  public txtacuerdotrue: boolean = false;
  public isLoading: boolean = true;
  public isAuthenticated: boolean = false;
  public isProcessing: boolean = false;
  public isProcessingCarrito: boolean = false;

  closeResult: string = '';
  public lbldescripcion: string = '';
  public lblunidadmedida: string = '';
  public lblcategoria: string = '';
  public lblmarca: string = '';
  public lblmarcaoriginal: string = '';
  public lblmedida: string = '';
  public lblmodelo: string = '';
  public lblmedidaestandarizado: string = '';
  public lblpathimagen: string = '';
  public lblid: string = '';
  public lblcodigo: string = '';
  public lblfamilia: string = '';
  public lblsubfamilia: string = '';
  public lbltipo: string = '';

  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public srcimg: string = 'assets/img/product/main-product/product6.webp';

  private wishlistSubscription!: Subscription | undefined;
  private cartSubscription!: Subscription | undefined;

  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Verifica si el scroll es mayor a 200px
    this.isVisible = window.scrollY > 200;
  }

  constructor(
    private route: ActivatedRoute,
    private maestroclasifi: MaestroclasificadoService,
    private router: Router,
    private auth: AuthService,
    private cotizacionService: CotizacionService,
    private modalService: NgbModal,
    private minoristaservice: MinoristaService
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshTokenvehiculo();
    this.route.params.subscribe((params) => {
      this.txtcategoria = params['marca'].toString() || '';
      localStorage.setItem('categoriavehiculo', this.txtcategoria);
    });
    this.initializeData();
    this.auth.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (this.isAuthenticated) {
        const txtuuid = this.decrypt(localStorage.getItem('uuid')!);
        this.loadWishlist(txtuuid);
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.cartItems = items;
          }
        );
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.wishlistItems = items;
          }
        ); // Cargar la wishlist desde la base de datos
      } else {
        // Suscribirse a los cambios en la wishlist
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.wishlistItems = items;
          }
        );
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.cartItems = items;
          }
        );
      }
    });
  }
  ngOnDestroy(): void {
    this.cartSubscription?.unsubscribe();
    this.wishlistSubscription?.unsubscribe();
  }

  private initializeData(): void {
    this.ListarGeneralVehiculo();
    this.ListarCategoriaVehiculo();
    this.ListarModeloVehiculo();
    this.ListarMarcaVehiculo();
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

  Paginacion() {
    localStorage.setItem('pvehiculo', this.p.toString());
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

  isInCarritolist(productIdentifier: string): boolean {
    if (this.isAuthenticated) {
      return this.cartItems.some(
        (item) => item.codigo.trim() === productIdentifier.trim()
      );
    } else {
      return this.cartItems.some((item) => item.codigo === productIdentifier);
    }
  }

  onImageLoad() {
    this.isLoading = false;
  }

  onImageError() {
    this.isLoading = false;
  }

  ListarCategoriaVehiculo() {
    this.maestroclasifi.getListadoCategoriaVehiculos().subscribe({
      next: (lst: any) => {
        this.ListarCategoria = lst;
      },
    });
  }

  ListarGeneralVehiculo() {
    const frombody: any = {
      categoria: this.txtcategoria ?? '',
      medida: this.txtmodelo ?? '',
      marca: this.txtmarca ?? '',
    };

    this.showAlert('Cargando Información....', 'info');
    this.maestroclasifi.getListadoGeneralVehiculos(frombody).subscribe({
      next: (lst: any) => {
        this.ListGeneral = lst.list;
      },
    });
  }

  ListarModeloVehiculo() {
    const frombody: any = {
      categoria: this.txtcategoria,
      marca: this.txtmarca,
    };
    this.maestroclasifi.getListadoModeloVehiculo(frombody).subscribe({
      next: (lst: any) => {
        this.ListModelo = lst.list;
      },
    });
  }

  ListarMarcaVehiculo() {
    const frombody: any = {
      categoria: this.txtcategoria,
      medida: this.txtmodelo,
    };
    this.maestroclasifi.getListarMarcaVehiculo(frombody).subscribe({
      next: (lst: any) => {
        this.ListMarca = lst.list;
      },
    });
  }

  SelectCategoria() {
    this.txtmarca = '';
    this.txtmodelo = '';
    this.router.navigate([`/homevehiculo/${this.txtcategoria}`]);
    this.ListarModeloVehiculo();
    this.ListarGeneralVehiculo();
    this.ListarMarcaVehiculo();
    localStorage.removeItem('marcavehiculo');
    localStorage.removeItem('modelovehiculo');
    localStorage.removeItem('pvehiculo');
  }

  Limpiar() {
    this.txtmarca = '';
    this.txtmodelo = '';
    localStorage.removeItem('marcavehiculo');
    localStorage.removeItem('modelovehiculo');
    localStorage.removeItem('pvehiculo');
    this.router.navigate([`/homevehiculo/${this.txtcategoria}`]);
    this.ListarModeloVehiculo();
    this.ListarGeneralVehiculo();
    this.ListarMarcaVehiculo();
  }
  SelectModelo() {
    this.ListarGeneralVehiculo();
    this.ListarMarcaVehiculo();
    localStorage.removeItem('pvehiculo');
    localStorage.setItem('modelovehiculo', this.txtmodelo);
  }

  SelectMarca() {
    this.ListarGeneralVehiculo();
    this.ListarModeloVehiculo();
    localStorage.removeItem('pvehiculo');
    localStorage.setItem('marcavehiculo', this.txtmarca);
  }

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
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

  AgregarCarritoModal() {
    // Crear el producto que se va a agregar
    if (this.isProcessingCarrito) return;
    this.isProcessingCarrito = true;

    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      if (this.isInCarritolist(this.lblcodigo.trim())) {
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
        pathimagen: this.lblpathimagen,
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

  clearVoid() {
    this.lbldescripcion = '';
    this.lblunidadmedida = '';
    this.lblcategoria = '';
    this.lblmarca = '';
    this.lblmarcaoriginal = '';
    //this.lblproducto = '';
    this.lblmedida = '';
    this.lblmodelo = '';
    this.lblmedidaestandarizado = '';
    this.lblpathimagen = '';
    this.lblid = '';
    this.lblcodigo = '';
    this.lblfamilia = '';
    this.lblsubfamilia = '';
    this.lbltipo = '';
    this.count = 1;
  }

  MdVehiculolistado(content: any, item: any) {
    const product = parseInt(item.id);
    this.wishlistItems.some((item) => item.id === product);
    this.clearVoid();
    this.lbldescripcion = item.descripcion;
    this.lblunidadmedida = item.unidadmedida;
    this.lblcategoria = item.categoria;
    this.lblmarca = item.marca;
    this.lblmarcaoriginal = item.marcaoriginal;
    //this.lblproducto = item.producto;
    this.lblmedida = item.medida;
    this.lblmodelo = item.modelo;
    this.lblmedidaestandarizado = item.medidaestandarizado;

    this.lblid = item.id;
    this.lblcodigo = item.codigo;
    this.lblfamilia = item.familia;
    this.lblsubfamilia = item.subfamilia;
    this.lbltipo = item.tipo;
    this.lblpathimagen = item.pathimagen;
    this.modalService
      .open(content, {
        windowClass: 'myCustomModalClass',
        centered: true,
        ariaLabelledBy: 'modal-basic-title',
        size: 'lg',
      })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissModal(reason)}`;
        }
      );
  }

  private getDismissModal(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
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
        quantity: this.count,
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

  MdWishListFavorito(item: any) {
    if (this.isProcessing) return; // Evitar múltiples clics
    this.isProcessing = true; // Establecer el estado de procesamiento

    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      // Verificar si el producto ya está en la wishlist usando item.codigo
      if (this.isInWishlist(item.codigo)) {
        // Si ya está en la wishlist, marcarlo como inactivo
        this.cotizacionService
          .removeFromWishlist(txtuuid, item.codigo.trim())
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
              const authProduct = this.createAuthProduct(item, dtl.orderNumber);
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
        quantity: this.count,
        color: '',
        pathimagen: item.pathimagen,
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

  private createAuthProduct(item: any, orderNumber: string): any {
    console.log(item)
    return {
      uuidcliente: this.decrypt(localStorage.getItem('uuid')!),
      ordenwishlist: orderNumber,
      descripcion: item.descripcion,
      unidad: item.unidadmedida,
      categoria: item.aplicacion,
      marca: item.marca,
      marcaoriginal: item.marcaoriginal,
      medida: item.medida,
      modelo: item.modelo,
      medidaestandarizado: item.medidaestandarizado,
      id: item.id,
      codigo: item.codigo.trim(),
      familia: item.familia,
      subfamilia: item.subfamilia,
      tipo: item.tipo,
      cantidad: 1,
      sku: item.codigo,
      producto: item.marcaoriginal,
      vendor: item.marca,
      quantity: this.count,
      color: '',
      pathimagen: item.pathimagen,
      estado: 'Activo',
      fecharegistro: new Date().toISOString(),
    };
  }
  private createAuthCarrito(item: any): any {
    return {
      uuidcliente: this.decrypt(localStorage.getItem('uuid')!),
      ordernshopping: '',
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
      quantity: this.count,
      color: '',
      pathimagen: item.pathimagen,
      estado: 'Activo',
      fecharegistro: new Date().toISOString(),
    };
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
