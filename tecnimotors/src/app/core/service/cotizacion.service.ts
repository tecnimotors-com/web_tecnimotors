import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CotizacionService {
  private cartItems: any[] = [];
  constructor() {}

  addToCart(product: any): void {
    const existingProductIndex = this.cartItems.findIndex(
      (item) => item.id === product.id
    );

    if (existingProductIndex !== -1) {
      // Si el producto ya existe, reemplaza la cantidad
      this.cartItems[existingProductIndex].cantidad = product.cantidad;
    } else {
      // Si el producto no existe, agregarlo al carrito
      this.cartItems.push(product);
    }
  }

  // Obtener los productos del carrito
  getCartItems(): any[] {
    return this.cartItems;
  }

  // Limpiar el carrito
  clearCart(): void {
    this.cartItems = [];
  }
}
