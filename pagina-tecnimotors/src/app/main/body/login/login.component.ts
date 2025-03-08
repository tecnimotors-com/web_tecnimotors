import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { SharedMain } from '../../sharedmain';
import { AuthService } from '../../../core/service/auth.service';
import Swal from 'sweetalert2';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';
import { CotizacionService } from '../../../core/service/cotizacion.service';
import CryptoJS from 'crypto-js';
import { environment } from '../../../../environments/environment.development';
import { Subscription } from 'rxjs';
import { MinoristaService } from '../../../core/service/marketing/minorista.service';

@Component({
  selector: 'app-login',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit, OnDestroy {
  public nombre: string = '';
  public apellido: string = '';
  public email: string = '';
  public celular: string = '';
  public password: string = '';
  public confirmPassword: string = '';
  public termsAccepted: boolean = false;
  public labelnombre: string = '';
  public labelpassword: string = '';
  public rememberMe: boolean = false;
  public isLoadinglogin: boolean = false;
  public showPassword: boolean = false;
  public showConfirmPassword: boolean = false;
  public showPasswordLogin: boolean = false;
  public isAuthenticated: boolean = false;
  public isLoadingRegister: boolean = false;
  private wishlistSubscription!: Subscription | undefined;
  private cartSubscription!: Subscription | undefined;
  public wishlistItems: any[] = [];
  public ListCarrito: any[] = [];
  public txtuuid: string = '';

  public ListCarritoRegister: any[] = [];
  public ListWishRegister: any[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private datePipe: DatePipe,
    private cotizacionService: CotizacionService,
    private minoristaservice: MinoristaService
  ) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  toggePasswordLogin() {
    this.showPasswordLogin = !this.showPasswordLogin;
  }

  ngOnInit(): void {
    this.authService.getRefreshToken();
    // Cargar el correo guardado si existe
    const savedEmail = localStorage.getItem('correo');
    if (savedEmail) {
      this.labelnombre = savedEmail;
      this.rememberMe = true; // Marcar el checkbox si hay un correo guardado
    }
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (this.isAuthenticated) {
        this.txtuuid = this.decrypt(localStorage.getItem('uuid')!);
        //this.loadWishlist(this.txtuuid);
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
            console.log(this.ListCarrito);
          }
        );
      }
    });
  }

  /*
  onLogin() {
    this.isLoadinglogin = true; // Activar el estado de carga
    const user = { correo: this.labelnombre, password: this.labelpassword };

    this.authService.logincliente(user).subscribe({
      next: (dtl) => {
        const encrypteduuid = this.encrypt(dtl.body.uuid);
        var returncategory = localStorage.getItem('returncategory');
        localStorage.setItem('uuid', encrypteduuid);

        this.authService.saveUsercliente(
          this.labelnombre,
          this.rememberMe,
          dtl.body.uuid
        );
        this.isLoadinglogin = false;

        if (returncategory) {
          this.router.navigate([returncategory]);
        } else {
          this.router.navigate(['/login']);
        }

        this.checkListsStatus();

        if (this.wishlistItems.length ==  0) {
          this.showAlert(dtl.msg, 'success');
          this.authService.saveUsercliente(
            this.labelnombre,
            this.rememberMe,
            dtl.body.uuid
          );
          this.isLoadinglogin = false;
          this.cotizacionService.clearWishlist();
        } else {
       
          this.authService.saveUsercliente(
            this.labelnombre,
            this.rememberMe,
            dtl.body.uuid
          );
          const frombody = {
            listWishLogin: this.wishlistItems,
            uuidCliente: dtl.body.uuid,
          };
          this.cotizacionService.getRegistrarWishlogin(frombody).subscribe({
            next: () => {
              this.wishlistSubscription =
                this.cotizacionService.wishlist$.subscribe((items) => {
                  this.wishlistItems = items;
                });
            
              this.showAlert('Se actualizo el listado de favorito', 'success');
            },
          });
        }
 
      },
      error: (err: any) => {
        var validad = err.error.msg == null ? '' : err.error.msg;
        if (validad == '') {
          this.showAlert('error de Credenciales', 'error');
          this.authService.logoutcliente();
          this.isLoadinglogin = false;
        } else {
          this.showAlert(validad, 'error');
          this.authService.logoutcliente();
          this.isLoadinglogin = false;
        }
      },
    });
  }
  */

  onLogin() {
    this.isLoadinglogin = true; // Activar el estado de carga
    const user = { correo: this.labelnombre, password: this.labelpassword };
    this.authService.logincliente(user).subscribe({
      next: (dtl) => {
        this.checkListsStatus(dtl.body.uuid);
        this.isLoadinglogin = false;
        this.authService.saveUsercliente(
          this.labelnombre,
          this.rememberMe,
          dtl.body.uuid
        );
        const encrypteduuid = this.encrypt(dtl.body.uuid);
        var returncategory = localStorage.getItem('returncategory');
        localStorage.setItem('uuid', encrypteduuid);

        if (returncategory) {
          this.loadWishlist(dtl.body.uuid);
          this.router.navigate([returncategory]);
        } else {
          this.loadWishlist(dtl.body.uuid);
          this.router.navigate(['/login']);
        }
      },
      error: (err: any) => {
        var validad = err.error.msg == null ? '' : err.error.msg;
        this.showAlert(validad || 'Error de Credenciales', 'error');
        this.authService.logoutcliente();
        this.isLoadinglogin = false; // Desactivar el estado de carga
      },
    });
  }

  private loadWishlist(txtuuid: string): void {
    this.cotizacionService.getAllWishlist(txtuuid).subscribe((dtl: any) => {
      this.wishlistItems = dtl;
    });
    this.cotizacionService.getAllCarritoList(txtuuid).subscribe((dtl: any) => {
      this.ListCarrito = dtl;
    });
  }

  private checkListsStatus(uuid: any): void {
    this.ListWishRegister = [];
    this.ListCarritoRegister = [];
    this.ListWishRegister = this.wishlistItems;
    this.ListCarritoRegister = this.ListCarrito;
    if (
      this.wishlistItems.length == 0 &&
      this.ListCarritoRegister.length == 0
    ) {
      this.isLoadinglogin = false;
      this.cotizacionService.clearWishlist();
      this.cotizacionService.clearCart2();
      this.cotizacionService.clearCart();
      this.loadWishlist(uuid);
    } else if (
      this.ListWishRegister.length == 0 &&
      this.ListCarritoRegister.length != 0
    ) {
      const frombody = {
        ListCarrito: this.ListCarritoRegister,
        uuidcliente: uuid,
      };
      console.log(frombody);
      this.cotizacionService.getRegisterCotizacionList(frombody).subscribe({
        next: () => {
          this.loadWishlist(uuid);
          this.showAlert('Se actualizo el listado de favorito', 'success');
        },
      });
    } else if (
      this.ListWishRegister.length != 0 &&
      this.ListCarritoRegister.length == 0
    ) {
      const frombody = {
        listWishLogin: this.ListWishRegister,
        uuidCliente: uuid,
      };
      this.cotizacionService.getRegistrarWishlogin(frombody).subscribe({
        next: () => {
          this.loadWishlist(uuid);
          this.showAlert('Se actualizo el listado de favorito', 'success');
        },
      });
    } else {
      const frombody = {
        uuidCliente: uuid,
        ListCarritoLogin: this.ListCarritoRegister,
        ListWishLogin: this.ListWishRegister,
      };

      this.cotizacionService.getRegistrarCarritoYWishlist(frombody).subscribe({
        next: () => {
          this.loadWishlist(uuid);
          this.showAlert(
            'Se actualizaron el carrito y la wishlist correctamente',
            'success'
          );
        },
        error: (err) => {
          console.error('Error al actualizar el carrito y la wishlist:', err);
        },
      });
    }
  }

  onForgotPassword() {
    // Lógica para manejar el olvido de contraseña
  }

  onRegister() {
    if (!this.nombre) {
      this.showAlert('Falta Agregar Nombre', 'error');
      return;
    }
    if (!this.apellido) {
      this.showAlert('Falta Agregar Apellido', 'error');
      return;
    }
    if (!this.email) {
      this.showAlert('Falta Agregar Correo', 'error');
      return;
    }
    if (!this.celular) {
      this.showAlert('Falta Agregar celular/telefono', 'error');
      return;
    }
    if (!this.password) {
      this.showAlert('Falta Agregar Password', 'error');
      return;
    }
    // Validar que las contraseñas coincidan
    if (this.password !== this.confirmPassword) {
      this.showAlert('Las contraseñas no coinciden.', 'error');
      return;
    }

    // Validar que se acepten los términos
    if (!this.termsAccepted) {
      this.showAlert('Debes aceptar los términos y condiciones.', 'error');
      return;
    }

    const user = {
      nombre: this.nombre,
      apellido: this.apellido,
      correo: this.email,
      celular: this.celular,
      password: this.password,
      repassword: this.confirmPassword,
      termaccept: this.termsAccepted,
      fecharegistro: this.datePipe.transform(new Date(), 'yyyy-MM-dd'),
      estado: 'Activo',
    };

    this.isLoadingRegister = true; // Activar el estado de carga

    // Llamar al servicio de registro
    this.authService.registercliente(user).subscribe({
      next: () => {
        this.showAlert(
          'Registro exitoso. Puedes iniciar sesión ahora.',
          'success'
        );
        this.isLoadingRegister = false;
        this.labelnombre = this.email;
        this.labelpassword = this.password;
        this.onLogin();
        this.clearRegister();
      },
      error: (err: any) => {
        this.showAlert(err.error.message, 'error');
        this.isLoadingRegister = false;
      },
      complete: () => {
        this.isLoadingRegister = false; // Desactivar el estado de carga al completar la solicitud
      },
    });
  }

  ngOnDestroy(): void {
    if (this.wishlistSubscription) {
      this.wishlistSubscription.unsubscribe();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
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

  clearRegister() {
    this.nombre = '';
    this.apellido = '';
    this.email = '';
    this.celular = '';
    this.password = '';
    this.confirmPassword = '';
    this.termsAccepted = false;
  }

  private encrypt(data: string): string {
    if (!data) {
      throw new Error('Data to encrypt cannot be null or undefined');
    }
    return CryptoJS.AES.encrypt(data, environment.apikeencriptado).toString();
  }

  private decrypt(data: string): string {
    if (!data) {
      return '';
    }
    const bytes = CryptoJS.AES.decrypt(data, environment.apikeencriptado);
    return bytes.toString(CryptoJS.enc.Utf8);
  }
}
