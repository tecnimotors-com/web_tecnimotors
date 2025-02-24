import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class CotizacionService {
  private cartItems: any[] = [];

  private acuerdoSubject = new BehaviorSubject<boolean>(false); // Inicializa con false
  acuerdo$ = this.acuerdoSubject.asObservable(); // Observable para suscribirse

  private cartSubject = new BehaviorSubject<any>(false);
  cart$ = this.cartSubject.asObservable();

  private wishlistItems: any[] = []; // Almacena los productos en la wishlist
  private wishlistSubject = new BehaviorSubject<any[]>(this.wishlistItems); // Para emitir cambios

  wishlist$ = this.wishlistSubject.asObservable(); // Observable para suscribirse a cambios

  constructor() {}

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
}
