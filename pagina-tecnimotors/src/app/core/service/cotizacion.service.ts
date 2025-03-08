import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import Swal from 'sweetalert2';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class CotizacionService {
  public myapitecni = environment.apimaestroarticulo;

  private acuerdoSubject = new BehaviorSubject<boolean>(false); // Inicializa con false
  acuerdo$ = this.acuerdoSubject.asObservable(); // Observable para suscribirse

  private cartItems: any[] = [];
  private cartSubject = new BehaviorSubject<any[]>(this.cartItems);
  public cart$ = this.cartSubject.asObservable();

  private wishlistItems: any[] = []; // Almacena los productos en la wishlist
  private wishlistSubject = new BehaviorSubject<any[]>(this.wishlistItems); // Para emitir cambios
  public wishlist$ = this.wishlistSubject.asObservable();

  public RegisterWishList = '/WishList/RegisterWishList';
  public ListadoWishList = '/WishList/ListadoWishList';
  public NextOrderNumber = '/WishList/NextOrderNumber';
  public removeWishList = '/WishList/removeWishList';
  public RegistrarWishlogin = '/WishList/RegistrarWishlogin';

  /*-----------Carrito----------------*/
  public ListadoCarritoList = '/CarritoCotizacion/ListadoCarritoList';
  public RegisterCarritoList = '/CarritoCotizacion/RegisterCarritoList';
  public RemoveCarritoList = '/CarritoCotizacion/RemoveCarritoList';
  public UpdateCantidadCarrito = '/CarritoCotizacion/UpdateCantidadCarrito';
  public RegisterCotizacionList = '/CarritoCotizacion/RegisterCotizacionList';
  public RegistrarCarritologin = '/CarritoCotizacion/RegistrarCarritologin';

  public DetailClienteUuid = '/Auth/DetailClienteUuid';
  public RegistrarCarritoYWishlist = '/CarritoLogin/RegistrarCarritoYWishlist';

  constructor(private http: HttpClient) {}

  // Método para guardar en la base de datos
  getRegisterWishList(product: any) {
    return this.http.post(this.myapitecni + this.RegisterWishList, product); // Cambia la URL según tu API
  }
  getNextOrderNumber2(uuidCliente: string): Observable<string> {
    return this.http.post<string>(`${this.myapitecni}${this.NextOrderNumber}`, {
      Uuidcliente: uuidCliente,
    });
  }

  getRegistrarWishlogin(frombody: any) {
    return this.http.post(this.myapitecni + this.RegistrarWishlogin, frombody);
  }

  getRegistrarCarritologin(frombody: any) {
    return this.http.post(
      this.myapitecni + this.RegistrarCarritologin,
      frombody
    );
  }
  getRegistrarCarritoYWishlist(frombody: any) {
    return this.http.post(
      this.myapitecni + this.RegistrarCarritoYWishlist,
      frombody
    );
  }
  // Método para obtener la wishlist desde la base de datos
  getAllWishlist(txtuuid: any) {
    const body = { Uuidcliente: txtuuid };
    return this.http
      .post<any[]>(this.myapitecni + this.ListadoWishList, body)
      .pipe(
        tap((items) => {
          this.wishlistItems = items; // Actualiza la lista local
          this.wishlistSubject.next(this.wishlistItems); // Emitir el nuevo estado de la wishlist
        })
      );
  }

  getAllCarritoList(txtuuid: any) {
    const body = { Uuidcliente: txtuuid };
    return this.http
      .post<any[]>(this.myapitecni + this.ListadoCarritoList, body)
      .pipe(
        tap((items) => {
          this.cartItems = items;
          this.cartSubject.next(this.cartItems);
        })
      );
  }

  getRegisterCarritoList(product: any) {
    return this.http.post(this.myapitecni + this.RegisterCarritoList, product);
  }
  // Método para eliminar un producto de la wishlist
  removeFromWishlist(uuidCliente: string, codigo: string): Observable<any> {
    const body = { Uuidcliente: uuidCliente, Codigo: codigo };
    return this.http
      .post(`${this.myapitecni}${this.removeWishList}`, body)
      .pipe(
        tap(() => {
          // Aquí puedes actualizar la wishlist local si es necesario
          this.wishlistItems = this.wishlistItems.filter(
            (item) => item.codigo !== codigo
          );
          this.wishlistSubject.next(this.wishlistItems); // Emitir el nuevo estado de la wishlist
        })
      );
  }
  UpdateCantidadCarritoList(
    uuidCliente: string,
    codigo: string,
    cantidad: number
  ): Observable<any> {
    const body = {
      Uuidcliente: uuidCliente,
      Codigo: codigo,
      Cantidad: cantidad,
    };
    return this.http
      .post(`${this.myapitecni}${this.UpdateCantidadCarrito}`, body)
      .pipe(
        tap(() => {
          // Aquí puedes actualizar la wishlist local si es necesario
          this.wishlistItems = this.wishlistItems.filter(
            (item) => item.codigo !== codigo
          );
          this.wishlistSubject.next(this.wishlistItems); // Emitir el nuevo estado de la wishlist
        })
      );
  }

  RemoveFromCarritoList(uuidCliente: string, codigo: string): Observable<any> {
    const body = { Uuidcliente: uuidCliente, Codigo: codigo };
    return this.http
      .post(`${this.myapitecni}${this.RemoveCarritoList}`, body)
      .pipe(
        tap(() => {
          this.cartItems = this.cartItems.filter(
            (item) => item.codigo !== codigo
          );
          this.cartSubject.next(this.cartItems);
        })
      );
  }

  addToCart2(product: any): void {
    const existingProductIndex = this.cartItems.findIndex(
      (item: any) => item.id === product.id
    );

    if (existingProductIndex !== -1) {
      // Si el producto ya existe, actualiza la cantidad
      this.cartItems[existingProductIndex].cantidad = product.cantidad;
      this.showAlert('Cantidad Actualizada en la Cotización', 'warning');
    } else {
      // Si el producto no existe, agrégalo al carrito
      this.cartItems.push(product);
      this.showAlert('Se Agregó a la Cotización', 'success');
    }

    // Emitir el nuevo estado del carrito
    this.cartSubject.next(this.cartItems);
  }

  addToCart(product: any): void {
    const existingProductIndex = this.cartItems.findIndex(
      (item) => item.id === product.id
    );

    if (existingProductIndex !== -1) {
      this.cartItems[existingProductIndex].cantidad = product.cantidad;
      this.showAlert('Cantidad Actualizada en la Cotización', 'warning');
    } else {
      this.cartItems.push(product);
      this.showAlert('Se Agrego a la Cotización', 'success');
    }
  }

  // Método para eliminar un producto del carrito
  eliminarProducto(item: any): void {
    this.cartItems = this.cartItems.filter(
      (product) => product.codigo !== item.codigo
    );
    this.cartSubject.next(this.cartItems); // Emitir el nuevo estado del carrito
    this.showAlert('Se Quitó de la Cotización', 'success');
  }
  // Obtener los productos del carrito
  getCartItems(): any[] {
    return this.cartItems;
  }

  // Limpiar el carrito
  clearCart(): void {
    this.cartItems = [];
  }

  getcaritem2(): any[] {
    return this.cartItems; // Devuelve la lista actual de productos en el carrito
  }

  clearCart2(): void {
    this.cartItems = []; // Limpia el carrito
    this.cartSubject.next(this.cartItems); // Emitir el nuevo estado del carrito (vacío)
  }

  // Método para agregar el estado del acuerdo
  addtoBoleanAcuerdo(acuerdo: boolean): void {
    this.acuerdoSubject.next(acuerdo); // Emitir el nuevo valor
  }

  // Método para obtener el estado del acuerdo
  getAcuerdo(): boolean {
    return this.acuerdoSubject.getValue(); // Devuelve el valor almacenado
  }

  clearAcuerdo(): void {
    this.acuerdoSubject.next(false); // Emitir false
  }

  /*----------------wish list---------------------*/
  addToWishlist(product: any): void {
    const existingProductIndex = this.wishlistItems.findIndex(
      (item: any) => item.id === product.id
    );

    if (existingProductIndex === -1) {
      // Si el producto no existe, agrégalo a la wishlist
      this.wishlistItems.push(product);
      this.showAlert('Se Agregó a la Lista de Favorito', 'success');
    } else {
      this.wishlistItems[existingProductIndex].cantidad = product.cantidad;
      this.showAlert('El producto ya está en la Lista de Favorito', 'warning');
    }

    // Emitir el nuevo estado de la wishlist
    this.wishlistSubject.next(this.wishlistItems);
  }

  // Método para eliminar un producto del carrito
  eliminarProductoWish(item: any): void {
    this.wishlistItems = this.wishlistItems.filter(
      (product) => product.codigo !== item.codigo
    );
    this.wishlistSubject.next(this.wishlistItems); // Emitir el nuevo estado del carrito
  }

  getWishlistItems(): any[] {
    return this.wishlistItems;
  }

  clearWishlist(): void {
    this.wishlistItems = []; // Limpia la wishlist
    this.wishlistSubject.next(this.wishlistItems); // Emitir el nuevo estado de la wishlist (vacío)
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

  getDetailClienteUuid(uuid: any) {
    return this.http.post(this.myapitecni + this.DetailClienteUuid, uuid); // Cambia la URL según tu API
  }

  getRegisterCotizacionList(frombody: any) {
    return this.http.post(this.myapitecni + this.RegisterCotizacionList,frombody);
  }
}
