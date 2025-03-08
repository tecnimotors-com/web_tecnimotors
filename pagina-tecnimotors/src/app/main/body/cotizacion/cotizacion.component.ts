import { SharedMain } from '../../sharedmain';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import Swal from 'sweetalert2';
import { MarketingService } from '../../../core/service/marketing.service';
import { MinoristaService } from '../../../core/service/marketing/minorista.service';
import { CotizacionService } from '../../../core/service/cotizacion.service';
import { Subscription } from 'rxjs';
import { DatePipe } from '@angular/common';
import { environment } from '../../../../environments/environment.development';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';
import CryptoJS from 'crypto-js';

@Component({
  selector: 'app-cotizacion',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './cotizacion.component.html',
  styleUrls: ['./cotizacion.component.scss'],
})
export class CotizacionComponent implements OnInit, OnDestroy {
  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public srcimg: string = 'assets/img/product/main-product/product6.webp';
  public txtnombre: string = '';
  public txtapellido: string = '';
  public txtcelular: string = '';
  public txtcorreo: string = '';
  public txtmensaje: string = '';
  public fecha: string = '';

  public departamento_id: string = '0';
  public provincia_id: string = '0';
  public distrito_id: string = '0';

  public listDepartamento: any[] = [];
  public listProvincia: any[] = [];
  public listDistrito: any[] = [];
  public ListCarrito: any[] = [];

  public ListAcuerdo: any[] = [];
  public wishlistItems: any[] = [];
  public cartItems: any[] = [];
  public cartList: any[] = [];

  // Inicializa la variable
  private acuerdoSubscription: Subscription | undefined; // Para manejar la suscripción
  private wishlistSubscription!: Subscription | undefined;
  private cartSubscription!: Subscription | undefined;

  public blnProducto: boolean = false;
  public isVisible: boolean = false;
  public isAuthenticated: boolean = false;
  public btnregistrar: boolean = false;
  public isProgressing: boolean = false;

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
    this.listarDepartamentos();

    this.auth.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (this.isAuthenticated) {
        const txtuuid = this.decrypt(localStorage.getItem('uuid')!);
        this.detailInformacion(txtuuid);
        this.loadWishlist(txtuuid);
        this.Loadlogin();
      } else {
        // Suscribirse a los cambios en la wishlist
        this.Loadlogin();
        this.ListCarrito = this.cotizacionService.getCartItems();
      }
    });
  }
  private Loadlogin() {
    this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
      (items) => {
        this.wishlistItems = items;
      }
    );
    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items;
    });
    this.acuerdoSubscription = this.cotizacionService.acuerdo$.subscribe(
      (acuerdo) => {
        this.blnProducto = acuerdo; // Actualiza blnProducto cuando cambia
      }
    );
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
  detailInformacion(uuid: string) {
    const frombody = {
      uuid: uuid,
    };
    this.cotizacionService.getDetailClienteUuid(frombody).subscribe({
      next: (dtl: any) => {
        this.txtnombre = dtl.nombre;
        this.txtapellido = dtl.apellido;
        this.txtcelular = dtl.celular;
        this.txtcorreo = dtl.correo;
      },
    });
  }
  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ngOnDestroy(): void {
    if (this.acuerdoSubscription) {
      this.acuerdoSubscription.unsubscribe();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  listarDepartamentos() {
    this.marketing.getListardepartamentos().subscribe({
      next: (value) => {
        this.listDepartamento = value;
      },
    });
  }

  listarProvincia() {
    const login: any = {
      departamento_id: this.departamento_id,
      provincia_id: this.provincia_id,
    };
    if (login.departamento_id == '') {
    }
    if (login.departamento_id == 0) {
      this.marketing.getListarProvincias(0).subscribe({
        next: (value) => {
          this.showAlert('Seleccione un departamento', 'info');
          this.listProvincia = value;
        },
      });
    }
    if (login.departamento_id == 0) {
      this.marketing.getListarDistritos(0, 0).subscribe({
        next: (value) => {
          this.listDistrito = value;
        },
      });
    } else if (login.departamento_id == login.departamento_id) {
      this.marketing.getListarProvincias(login.departamento_id).subscribe({
        next: (value) => {
          this.listProvincia = value;
        },
      });
    }
  }

  listarDistrito() {
    const login: any = {
      departamento_id: this.departamento_id,
      provincia_id: this.provincia_id,
    };
    if (login.departamento_id == '' && login.provincia_id == '') {
      //this.listDistrito = [];
    } else if (login.departamento_id == 0 && login.provincia_id == 0) {
      this.marketing.getListarDistritos(0, 0).subscribe({
        next: (value) => {
          this.listDistrito = value;
        },
      });
    } else if (login.provincia_id == 0) {
      this.marketing.getListarDistritos(0, 0).subscribe({
        next: (value) => {
          this.listDistrito = value;
        },
      });
    } else if (
      login.departamento_id == login.departamento_id &&
      login.provincia_id == login.provincia_id
    ) {
      this.marketing
        .getListarDistritos(login.departamento_id, login.provincia_id)
        .subscribe({
          next: (value) => {
            this.listDistrito = value;
          },
        });
    }
  }

  AnclarDeAcuerdo() {
    this.cotizacionService.addtoBoleanAcuerdo(this.blnProducto); // Envía el valor booleano al servicio
  }

  // type AlertType = 'success' | 'error' | 'info';
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

  private createMinoristaObject() {
    return {
      firstname: this.txtnombre,
      celular: this.txtcelular,
      email: this.txtcorreo,
      acuerdo: this.blnProducto,
    };
  }
  EnviarCoti() {
    this.btnregistrar = true;
    const minorista = this.createMinoristaObject();

    if (!minorista.firstname) {
      this.btnregistrar = false;
      this.showAlert('Nombre Vacio ', 'error');
      return;
    }
    if (!minorista.celular) {
      this.btnregistrar = false;
      this.showAlert('Celular Vacio ', 'error');
      return;
    }
    if (!minorista.email) {
      this.btnregistrar = false;
      this.showAlert('Correo Vacio ', 'error');
      return;
    }
    if (!minorista.acuerdo) {
      this.btnregistrar = false;
      this.showAlert('Check faltante Vacio ', 'error');
      return;
    }
    if (this.ListCarrito.length === 0) {
      this.btnregistrar = false;
      this.showAlert('Listado Producto Vacio ', 'error');
      return;
    }

    if (this.isProgressing) return;
    this.isProgressing = true;
    this.showAlert('Cargando Información...', 'info');
    this.minoristaservice.getObtenerUltimoRegistro().subscribe({
      next: (lst: any) => {
        this.fecha = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;
        this.cartList = this.ListCarrito;

        const txtuuid = this.decrypt(localStorage.getItem('uuid')!);

        const bodyCoti: any = {
          ListCarritoLogin: this.cartList,
          Quote: lst.quote,
          Uuidcliente: txtuuid,
        };
        
        this.cotizacionService.getRegistrarCarritologin(bodyCoti).subscribe({
          next: () => {

            // Agregar la propiedad 'quote' a cada elemento de ListCarrito
            this.ListCarrito = this.ListCarrito.map((item) => ({
              ...item, // Copia todas las propiedades del objeto original
              quote: lst.quote, // Agrega la nueva propiedad 'quote'
            }));

            const bodyform: any = {
              quote: lst.quote,
              firstname: this.txtnombre,
              lastname: this.txtapellido,
              celular: this.txtcelular,
              email: this.txtcorreo,
              mensaje: this.txtmensaje,
              fuente_id: 1,
              estado_id: 1,
              fecharegistro: this.fecha,
              vendedores: '',
              listProduct: this.ListCarrito,
            };

            this.minoristaservice.getRegistroMinoristaAll(bodyform).subscribe({
              next: () => {
                this.showAlert('Se registro exitosamente ', 'success');
                this.voidLimpiar();
                this.ListCarrito = [];
                this.cotizacionService.clearAcuerdo();
                this.cotizacionService.clearCart2();
                this.isProgressing = false;
              },
              error: () => {
                this.showAlert('Error, Volver a Enviar Cotización', 'error');
                this.isProgressing = false;
              },
            });
          },
          error: () => {
            this.showAlert('Error, Volver a Enviar Cotización', 'error');
            this.isProgressing = false;
          },
        });
      },
    });
  }
  voidLimpiar() {
    this.txtnombre = '';
    this.txtapellido = '';
    this.txtcelular = '';
    this.txtcorreo = '';
    this.txtmensaje = '';
    this.departamento_id = '0';
    this.provincia_id = '0';
    this.distrito_id = '0';
    this.listDepartamento = [];
    this.listProvincia = [];
    this.listDistrito = [];
    this.blnProducto = false;
  }
}
