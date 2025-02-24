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

@Component({
  selector: 'app-cotizacion',
  imports: [SharedMain],
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

  public departamento_id: string = '0';
  public provincia_id: string = '0';
  public distrito_id: string = '0';

  public listDepartamento: any[] = [];
  public listProvincia: any[] = [];
  public listDistrito: any[] = [];
  public ListCarrito: any[] = [];

  public blnProducto: boolean = false; // Inicializa la variable
  private acuerdoSubscription: Subscription | undefined; // Para manejar la suscripción
  private cartSubscription!: Subscription | undefined; // Para manejar la suscripción

  public isVisible: boolean = false;

  public ListAcuerdo: any[] = [];
  public fecha: string = '';

  public btnregistrar: boolean = false;

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

    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items; // Actualiza la lista de productos en el carrito
    });

    this.acuerdoSubscription = this.cotizacionService.acuerdo$.subscribe(
      (acuerdo) => {
        this.blnProducto = acuerdo; // Actualiza blnProducto cuando cambia
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
    if (this.acuerdoSubscription) {
      this.acuerdoSubscription.unsubscribe();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
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
  showAlert(texto: string, type: any) {
    Swal.fire({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
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

    this.minoristaservice.getObtenerUltimoRegistro().subscribe({
      next: (lst: any) => {
        this.fecha = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;

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
