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

@Component({
  selector: 'app-listadovehiculo',
  imports: [SharedMain],
  templateUrl: './listadovehiculo.component.html',
  styleUrls: ['./listadovehiculo.component.scss'],
})
export class ListadovehiculoComponent implements OnInit, OnDestroy {
  public ListarCategoria: any[] = [];
  public ListModelo: any[] = [];
  public ListMarca: any[] = [];

  public ListGeneral: any[] = [];
  public txtcategoria: string = '';
  public txtmodelo: string = '';
  public txtmarca: string = '';

  public p: number = 1;
  public itemper: number = 10;

  public ListCarrito: any[] = [];
  public isVisible: boolean = false;
  closeResult: string = '';

  public lbldescripcion: string = '';
  public lblunidadmedida: string = '';
  public lblcategoria: string = '';
  public lblmarca: string = '';
  public lblmarcaoriginal: string = '';
  //public lblproducto: string = '';
  public lblmedida: string = '';
  public lblmodelo: string = '';
  public lblmedidaestandarizado: string = '';
  public lblpathimagen: string = '';
  public lblid: string = '';
  public lblcodigo: string = '';
  public lblfamilia: string = '';
  public lblsubfamilia: string = '';
  public lbltipo: string = '';

  public count: number = 1;
  public blndisable = false;
  public txtacuerdotrue: boolean = false;
  private cartSubscription!: Subscription | undefined; // Para manejar la suscripción
  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public srcimg: string = 'assets/img/product/main-product/product6.webp';

  public isLoading: boolean = true;

  private wishlistSubscription!: Subscription | undefined;
  public ListWishlist: any[] = [];
  public wishlistItems: any[] = [];

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
    this.auth.getRefreshToken();
    this.route.params.subscribe((params) => {
      this.txtcategoria = params['marca'].toString()!;
    });
    this.ListarGeneralVehiculo();
    this.ListarCategoriaVehiculo();
    this.ListarModeloVehiculo();
    this.ListarMarcaVehiculo();
    this.initializePreLoader();
    // Suscribirse a los cambios en la wishlist
    this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
      (items) => {
        this.wishlistItems = items; // Actualiza la lista de productos en la wishlist
      }
    );
    setTimeout(() => {
      this.finalizePreLoader();
    }, 1000);
  }

  isInWishlist(productId: number): boolean {
    return this.wishlistItems.some((item) => item.id === productId);
  }

  isInWishlist2(productId: string): boolean {
    const product = parseInt(productId);
    return this.wishlistItems.some((item) => item.id === product);
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
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
  }

  Limpiar() {
    this.txtmarca = '';
    this.txtmodelo = '';
    this.router.navigate([`/homevehiculo/${this.txtcategoria}`]);
    this.ListarModeloVehiculo();
    this.ListarGeneralVehiculo();
    this.ListarMarcaVehiculo();
  }
  SelectModelo() {
    this.ListarGeneralVehiculo();
    this.ListarMarcaVehiculo();
  }

  SelectMarca() {
    this.ListarGeneralVehiculo();
    this.ListarModeloVehiculo();
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

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // type AlertType = 'success' | 'error' | 'info';
  showAlert(texto: string, type: any) {
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
      quantity: this.count,
      color: '',
      pathimagen: item.pathimagen,
    };
    this.cotizacionService.addToCart2(product);
    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items; // Actualiza la lista de productos en el carrito
    });
  }
  AgregarCarritoModal() {
    // Crear el producto que se va a agregar
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
    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items;
    });

    /*
    this.cotizacionService.addToCart(product);
    this.ListCarrito = this.cotizacionService.getCartItems();
    */
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

  MdWishListFavorito(item: any) {
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
        this.ListWishlist = items; // Actualiza la lista de productos en el carrito
      }
    );
  }
}
