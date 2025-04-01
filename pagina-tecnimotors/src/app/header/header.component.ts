import { Component, HostListener, OnInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { CotizacionService } from '../core/service/cotizacion.service';
import { SharedHeader } from './sharedheader';
import { Subscription } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { AuthService } from '../core/service/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import CryptoJS from 'crypto-js';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [SharedHeader],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  animations: [
    trigger('toggleSearch', [
      state(
        'hidden',
        style({
          opacity: 0,
          height: '0px',
          overflow: 'hidden',
        })
      ),
      state(
        'visible',
        style({
          opacity: 1,
          height: '*',
        })
      ),
      transition('hidden => visible', [animate('300ms ease-in')]),
      transition('visible => hidden', [animate('300ms ease-out')]),
    ]),
  ],
})
export class HeaderComponent implements OnInit {
  isAuthenticated: boolean = false;
  userEmail: string | null = null;

  public isOpenCarShop: boolean = false;
  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public srcimg: string = 'assets/img/product/main-product/product6.webp';
  public isOpen: boolean = false;
  public correo: string = 'ventas@tecnimotors.com';
  public isSticky: boolean = false;
  public isSubMenuOpen: boolean = false;
  public isSubMenuOpen2: boolean = false;
  public divstyle: string =
    'display: flex;font-size: 11px;font-weight: 600;justify-content: space-between;padding: 8px 10px;color:black';
  public titlestyle: string =
    /*"background-color: rgb(181, 181, 181);font-family: 'Rubik';font-size: 11px;font-weight: 600;padding: 8px 12px;";*/
    'background-color: #474747;font-size: 11px;font-weight: 600;padding: 8px 12px;color: white;text-decoration: underline;';

  //public carritoAbierto: boolean = false;

  public motoscycle: any[] = [
    {
      title: 'VEHICULOS',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/homevehiculo/Motocicleta',
    },
    {
      title: 'LLANTAS Y CÁMARAS',
      isOpen: false,
      style: this.divstyle,
      subtitle: [
        {
          subtitle: 'LLANTAS',
          isOpen: false,
          style: this.divstyle,
          routerlink: '/homellantas',
        },
        {
          subtitle: 'CÁMARAS',
          isOpen: false,
          style: this.divstyle,
          routerlink: '/homecamara',
        },
      ],
    },
    {
      title: 'RESPUESTOS',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/homerepuesto',
    },
    {
      title: 'ACEITES Y LUBRICANTES',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/homeaceite',
    },
    /*
    { title: 'ACCESORIOS', isOpen: false, style: this.divstyle, subtitle: [] },
     */
  ];

  public blog: any[] = [
    {
      title: 'NOSOTROS',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/nosotros',
    },
  ];

  public hdnSearch: boolean = true;
  public ListCarrito: any[] = [];
  public txtacuerdotrue: boolean = false; // Estado del checkbox
  private acuerdoSubscription!: Subscription | undefined; // Para manejar la suscripción
  private cartSubscription!: Subscription | undefined;

  public Listwishlist: any[] = [];
  private wishlistSubscription!: Subscription | undefined;

  public isLoadinglogin: boolean = false;
  public labelnombre: string = '';
  public labelpassword: string = '';
  public rememberMe: boolean = false;
  public showPasswordLogin: boolean = false;
  private modalRefEstado: any;
  public currentUrl: string = '';

  public isProcessingCarrito: boolean = false;
  public isProcessingdiminuir: boolean = false;
  public isProcessingaAumentar: boolean = false;
  public isDropdownOpen: boolean = false;
  
  constructor(
    private cotizacionService: CotizacionService,
    private authService: AuthService,
    private router: Router,
    private modalService: NgbModal,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // Capturar la URL completa
    this.currentUrl = this.router.url;

    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (this.isAuthenticated) {
        const txtuuid = this.decrypt(localStorage.getItem('uuid')!);
        this.userEmail = this.authService.getUsercliente();
        this.loadWishlist(txtuuid);
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.Listwishlist = items;
          }
        );
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.ListCarrito = items;
          }
        );
      } else {
        this.wishlistSubscription = this.cotizacionService.wishlist$.subscribe(
          (items) => {
            this.Listwishlist = items; // Actualiza la lista de productos en la wishlist
          }
        );
        this.cartSubscription = this.cotizacionService.cart$.subscribe(
          (items) => {
            this.ListCarrito = items; // Actualiza la lista de productos en el carrito
          }
        );
      }
    });

    this.checkAuthentication();

    this.acuerdoSubscription = this.cotizacionService.acuerdo$.subscribe(
      (acuerdo) => {
        this.txtacuerdotrue = acuerdo; // Actualiza blnProducto cuando cambia
      }
    );
  }

  private loadWishlist(txtuuid: string): void {
    this.cotizacionService.getAllWishlist(txtuuid).subscribe((dtl: any) => {
      this.Listwishlist = dtl;
    });
    this.cotizacionService.getAllCarritoList(txtuuid).subscribe((dtl: any) => {
      this.ListCarrito = dtl;
    });
  }

  checkAuthentication() {
    this.isAuthenticated = this.authService.isAuthenticatedcliente();
    if (this.isAuthenticated) {
      this.userEmail = this.authService.getUsercliente(); // Obtener el correo del usuario
    }
  }

  handleWishClick(content: any) {
    //this.authService.getRefreshToken();
    if (this.isAuthenticated) {
      // Si el usuario está autenticado, redirigir a la página de perfil
      this.router.navigate(['/wishlist']);
    } else {
      var res = this.authService.getDesconvertir();
      // Si no está autenticado, abrir el modal de inicio de sesión
      this.modalRefEstado = this.modalService.open(content, {
        windowClass: 'myCustomModalClass',
        centered: true,
        ariaLabelledBy: 'modal-basic-title',
        size: 'sm',
      });
    }
  }

  onLogout() {
    this.authService.logoutcliente();
    this.isAuthenticated = false;
    this.userEmail = null;
    this.cotizacionService.clearWishlist();
    this.cotizacionService.clearCart();
    this.cotizacionService.clearCart2();
    this.cotizacionService.clearAcuerdo();
    if (this.currentUrl) {
      localStorage.setItem('returncategory', this.currentUrl.toString());
      this.router.navigate([this.currentUrl.toString()]);
    } else {
      localStorage.setItem('returncategory', this.currentUrl.toString());
      this.router.navigate(['/login']);
    }
  }

  Cotizacion() {
    if ((this.txtacuerdotrue = true)) {
      if (this.isAuthenticated) {
        localStorage.setItem('returncategory', this.currentUrl.toString());
        this.router.navigate(['/cotizacion']);
      } else {
        localStorage.setItem('returncategory', this.currentUrl.toString());
        this.router.navigate(['/login']);
      }
    } else {
      this.showAlert(
        'Tiene que aceptar de Acuerdo de los productos Seleccionados',
        'info'
      );
    }
  }
  authfavorito() {
    if (this.isAuthenticated) {
      localStorage.setItem('returncategory', this.currentUrl.toString());
      this.router.navigate(['/wishlist']);
    } else {
      localStorage.setItem('returncategory', this.currentUrl.toString());
      this.router.navigate(['/login']);
    }
  }

  toggleblogmenu(menuItem: any) {
    const isCurrentlyOpen = menuItem.isOpen;
    this.blog.forEach((item: any) => {
      item.isOpen = false;
      item.isSelected = false;
    });
    if (!isCurrentlyOpen) {
      menuItem.isOpen = true;
      menuItem.isSelected = true;
    }
  }

  toggleMenuprueba(menuItem: any) {
    const isCurrentlyOpen = menuItem.isOpen;
    this.motoscycle.forEach((item: any) => {
      item.isOpen = false;
      item.isSelected = false;
    });
    if (!isCurrentlyOpen) {
      menuItem.isOpen = true;
      menuItem.isSelected = true;
    }
  }

  toggleSubMenuprueba(subItem: any) {
    subItem.isOpen = !subItem.isOpen;

    this.motoscycle.forEach((menu) => {
      // Verifica si menu.subtitle está definido y es un array
      if (Array.isArray(menu.subtitle)) {
        menu.subtitle.forEach((item: any) => {
          if (item !== subItem) {
            item.isOpen = false; // Cerrar otros subelementos
            item.isSelected = false; // Desmarcar otros subelementos
          } else {
            item.isSelected = !item.isSelected; // Alternar selección del subItem
          }
        });
      }
    });
  }

  toggleSubsubMenuprueba(subSubItem: any) {
    const parentMenu = this.motoscycle.find((item: any) =>
      item.subtitle.some(
        (sub: any) =>
          Array.isArray(sub.subsubtitle2) &&
          sub.subsubtitle2.includes(subSubItem)
      )
    );

    if (parentMenu) {
      parentMenu.subtitle.forEach((sub: any) => {
        if (Array.isArray(sub.subsubtitle2)) {
          sub.subsubtitle2.forEach((item: any) => {
            if (item !== subSubItem) {
              item.isOpen = false;
              item.isSelected = false;
            }
          });
        }
      });
    }
    subSubItem.isOpen = !subSubItem.isOpen;
    subSubItem.isSelected = !subSubItem.isSelected;
  }

  togglesubmenusegundo(thirdtitle: any) {
    this.motoscycle.forEach((menu) => {
      menu.subtitle.forEach((subItem: any) => {
        if (subItem.thirdtitle) {
          subItem.thirdtitle.forEach((item: any) => {
            item.isSelected = false;
          });
        }
      });
    });
    thirdtitle.isSelected = true;
  }

  ngOnDestroy(): void {
    if (this.acuerdoSubscription) {
      this.acuerdoSubscription.unsubscribe();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
    if (this.wishlistSubscription) {
      this.wishlistSubscription.unsubscribe();
    }
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isSticky = window.scrollY > 0; // Cambia esto según tu necesidad
  }

  abrirCarrito(event: MouseEvent): void {
    event.stopPropagation(); // Evita que el evento de clic se propague
    this.isOpenCarShop = !this.isOpenCarShop; // Alterna el estado del carrito
    this.isOpen = false; // Cierra el otro offcanvas si está abierto
    this.ListCarrito = this.cotizacionService.getCartItems();
  }

  cerrarCarrito(): void {
    this.isOpenCarShop = false; // Cierra el carrito
  }

  toggleOffcanvas(event: MouseEvent): void {
    event.stopPropagation(); // Evita que el evento de clic se propague
    this.isOpen = !this.isOpen; // Alterna el estado del otro offcanvas
    this.isOpenCarShop = false; // Cierra el carrito si está abierto
  }
  // Método para prevenir el cierre al hacer clic dentro del offcanvas
  preventClose(event: MouseEvent): void {
    event.stopPropagation(); // Previene que el clic se propague al documento
  }

  closeOffcanvas(): void {
    this.isOpen = false; // Cierra el otro offcanvas
  }

  clicksearch(): void {
    this.hdnSearch = !this.hdnSearch;
  }
  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen; // Alternar el estado del menú
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;

    // Verifica si el clic se realizó dentro del offcanvas o del botón de abrir
    const isClickInsideOffcanvas = target.closest('.offcanvas__header');
    const isClickInsideToggleButton = target.closest(
      '.offcanvas__header--menu__open--btn'
    );
    // Cierra el offcanvas si se hace clic fuera de él
    if (this.isOpen && !isClickInsideOffcanvas && !isClickInsideToggleButton) {
      this.closeOffcanvas();
    }

    // Verifica si el clic se realizó dentro del carrito
    const isClickInsideMinicart = target.closest('.offcanvas__minicart');
    const isclickInsideToggleButton = target.closest(
      '.offcanvas__header--menu__open--btn2'
    );
    if (this.isOpenCarShop && !isclickInsideToggleButton) {
      this.cerrarCarrito();
    }
    // Cierra el submenú si se hace clic fuera de él
    if (
      !target.closest('.header__sub--menu') &&
      !target.closest('.header__menu--items')
    ) {
      this.isSubMenuOpen = false; // Cerrar el submenú
    }

    // Cierra el menú desplegable si se hace clic fuera de él
    if (
      !target.closest('.header__sub--menu') &&
      !target.closest('.header__menu--link')
    ) {
      this.isDropdownOpen = false; // Cerrar el menú desplegable
    }
    // Cierra el submenú si se hace clic fuera de él
    if (
      !target.closest('.header__sub--menu') &&
      !target.closest('.header__menu--items')
    ) {
      this.isSubMenuOpen = false;
    }
  }

  get searchState() {
    return this.hdnSearch ? 'hidden' : 'visible';
  }

  // Método para aumentar la cantidad
  aumentarCantidad(item: any): void {
    if (this.isProcessingaAumentar) return;
    this.isProcessingaAumentar = true;

    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      // Validar que la cantidad no supere el límite de 10
      if (item.cantidad < 10) {
        // Aumentar la cantidad en la base de datos
        this.cotizacionService
          .UpdateCantidadCarritoList(
            txtuuid,
            item.codigo.trim(),
            item.cantidad + 1
          ) // Aumenta la cantidad en 1
          .subscribe({
            next: () => {
              this.loadWishlist(txtuuid);
              this.showAlert('Cantidad Actualizada', 'success');
              this.isProcessingaAumentar = false;
            },
            error: () => {
              this.isProcessingaAumentar = false;
              this.showAlert('Error al actualizar la cantidad', 'error');
            },
          });
      } else {
        this.isProcessingaAumentar = false;
        this.showAlert('La cantidad máxima permitida es 10', 'warning');
      }
    } else {
      if (item.cantidad < 10) {
        const existingProductIndex = this.ListCarrito.findIndex(
          (product) => product.codigo === item.codigo
        );
        if (existingProductIndex !== -1) {
          this.ListCarrito[existingProductIndex].cantidad += 1; // Aumenta la cantidad
        }
        this.isProcessingaAumentar = false;
      } else {
        this.isProcessingaAumentar = false;
        this.showAlert('La cantidad máxima permitida es 10', 'warning');
      }
    }
  }

  // Método para disminuir la cantidad
  disminuirCantidad(item: any): void {
    if (this.isProcessingdiminuir) return;
    this.isProcessingdiminuir = true;

    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
      // Validar que la cantidad no sea menor que 1
      if (item.cantidad > 1) {
        // Disminuir la cantidad en la base de datos
        this.cotizacionService
          .UpdateCantidadCarritoList(
            txtuuid,
            item.codigo.trim(),
            item.cantidad - 1
          ) // Disminuye la cantidad en 1
          .subscribe({
            next: () => {
              this.isProcessingdiminuir = false;
              this.loadWishlist(txtuuid);
              this.showAlert('Cantidad Actualizada', 'success');
            },
            error: () => {
              this.isProcessingdiminuir = false;
              this.showAlert('Error al actualizar la cantidad', 'error');
            },
          });
      } else {
        this.isProcessingdiminuir = false;
        this.showAlert('La cantidad mínima permitida es 1', 'warning');
      }
    } else {
      if (item.cantidad > 1) {
        const existingProductIndex = this.ListCarrito.findIndex(
          (product) => product.codigo === item.codigo
        );
        if (existingProductIndex !== -1) {
          if (this.ListCarrito[existingProductIndex].cantidad > 1) {
            this.ListCarrito[existingProductIndex].cantidad -= 1; // Disminuye la cantidad
          }
        }
        this.isProcessingdiminuir = false;
      } else {
        this.isProcessingdiminuir = false;
        this.showAlert('La cantidad mínima permitida es 1', 'warning');
      }
    }
  }

  // Método para eliminar un producto del carrito
  eliminarProducto(item: any): void {
    if (this.isProcessingCarrito) return;
    this.isProcessingCarrito = true;

    if (this.isAuthenticated) {
      var txtuuid = this.decrypt(localStorage.getItem('uuid')!);
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
            this.loadWishlist(txtuuid);
            this.isProcessingCarrito = false;
            this.showAlert(
              'Error al eliminar a la lista de cotización',
              'error'
            );
          },
        });
    } else {
      this.ListCarrito = this.ListCarrito.filter(
        (product) => product.codigo !== item.codigo
      );
      this.cotizacionService.eliminarProducto(item);
      this.isProcessingCarrito = false;
    }
  }

  AnclarDeAcuerdo() {
    this.cotizacionService.addtoBoleanAcuerdo(this.txtacuerdotrue);
    // Envía el valor booleano al servicio
  }

  toggePasswordLogin() {
    this.showPasswordLogin = !this.showPasswordLogin;
  }

  onLogin() {
    this.isLoadinglogin = true; // Activar el estado de carga
    const user = { correo: this.labelnombre, password: this.labelpassword };

    this.authService.logincliente(user).subscribe({
      next: (dtl) => {
        this.showAlert(dtl.msg, 'success');
        this.authService.saveUsercliente(
          this.labelnombre,
          this.rememberMe,
          dtl.body.uuid
        );
        this.isLoadinglogin = false;
        this.router.navigateByUrl('/wishlist');
        this.modalRefEstado.close('Save click');
      },
      error: (err: any) => {
        var validad = err.error.msg == null ? '' : err.error.msg;
        if (validad == '') {
          this.showAlert('error de Credenciales', 'error');
          //this.authService.logoutcliente();
          this.isLoadinglogin = false;
        } else {
          this.showAlert(validad, 'error');
          //this.authService.logoutcliente();
          this.isLoadinglogin = false;
        }
      },
      complete: () => {
        this.isLoadinglogin = false; // Desactivar el estado de carga al completar la solicitud
      },
    });
  }

  private decrypt(data: string): string {
    if (!data) {
      return '';
    }
    const bytes = CryptoJS.AES.decrypt(data, environment.apikeencriptado);
    return bytes.toString(CryptoJS.enc.Utf8);
  }

  ClickCrear() {
    this.router.navigateByUrl('/login');

    this.modalRefEstado.close('Save click');
  }

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
}
