import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class MinoristaService {
  private baseUrl = environment.myapimarketing;

  private ListarMinorista = '/Minorista/ListarMinorista';
  private registrarMinorista = '/Minorista/RegistrarMinorista';
  private detalleminorista = '/Minorista/DetalleMinorista';

  private registrarProducto = '/Minorista/RegistrarDetalleProducto';
  private listarDetalleProducto = '/Minorista/ListarDetalleProducto';
  private ActualizarVendedorMinorista =
    '/Minorista/ActualizarVendedorMinorista';
  private detalleEditableMinorista = '/Minorista/DetalleEditableMinorista';
  private ActualizarMinorista = '/Minorista/ActualizarMinorista';
  private duplicidadMinorista = '/Minorista/DuplicidadMinorista';
  private DetalleProducto = '/Minorista/DetalleProducto';
  private ActualizarProducto = '/Minorista/ActualizarProducto';

  /*----------------- ---------------------*/
  private TotalMinoristaPendiente = '/Minorista/TotalMinoristaPendiente';
  private TotalMinorista = '/Minorista/TotalMinorista';
  private ListarEstadoMinorista = '/EstadoMayorista/ListarEstadoMayorista';
  private ListarMinoristaporEstado = '/Minorista/ListarMinoristaporEstado';
  private UltimoRegistroNroCotizacion = '/Minorista/ObtenerUltimoNrocotizacion';
  private ObtenerMinorista = '/Minorista/ObtenerMinorista';
  private ListarFuente = '/Fuentes/ListarFuentes';
  private ListarMinoristaBuscar = '/Minorista/ListarMinoristaBuscar';
  private DeleteProductoCliente = '/Minorista/DeleteMinorista';
  private EliminarMinorista = '/Minorista/EliminarMinorista/';
  private EliminarMinoristaDelete = '/Minorista/EliminarMinorista/';
  private ObtenerUltimoRegistro = '/Minorista/ObtenerUltimoRegistro';
  private RegistroMinoristaAll = '/Minorista/RegistroMinoristaAll';
  private DetailMinoristaAll = '/Minorista/DetailMinoristaAll/';

  constructor(private http: HttpClient) {}
  getListarMinoristaBuscar(quote: string): Observable<any> {
    return this.http.get(
      this.baseUrl + this.ListarMinoristaBuscar + '/' + quote
    );
  }

  getlistMinorista(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarMinorista);
  }
  getRegistrarMinorista(Minorista: any): Observable<any> {
    return this.http.post(this.baseUrl + this.registrarMinorista, Minorista);
  }
  getdetalleminorista(quote: string): Observable<any> {
    return this.http.get(this.baseUrl + this.detalleminorista + '/' + quote);
  }
  getDetalleProducto(id: number): Observable<any> {
    return this.http.get(this.baseUrl + this.DetalleProducto + '/' + id);
  }

  getregistrarProducto(detalle: any): Observable<any> {
    return this.http.post(this.baseUrl + this.registrarProducto, detalle);
  }
  getlistarDetalleProducto(quote: string): Observable<any> {
    return this.http.get(
      this.baseUrl + this.listarDetalleProducto + '/' + quote
    );
  }
  getActualizarVendedorMinorista(Minorista: any): Observable<any> {
    return this.http.put(
      this.baseUrl + this.ActualizarVendedorMinorista,
      Minorista
    );
  }
  getdetalleEditableMinorista(quote: string): Observable<any> {
    return this.http.get(
      this.baseUrl + this.detalleEditableMinorista + '/' + quote
    );
  }
  getActualizarMinorista(Minorista: any): Observable<any> {
    return this.http.put(this.baseUrl + this.ActualizarMinorista, Minorista);
  }
  getActualizarProducto(producto: any): Observable<any> {
    return this.http.put(this.baseUrl + this.ActualizarProducto, producto);
  }

  getDeleteProductoCliente(id: number): Observable<any> {
    return this.http.get(this.baseUrl + this.DeleteProductoCliente + '/' + id);
  }

  getduplicidadMinorista(quote: string): Observable<any> {
    return this.http.get(this.baseUrl + this.duplicidadMinorista + '/' + quote);
  }

  /*------------------------------------------------------------------------*/

  getTotalMinoristaPendiente(): Observable<any> {
    return this.http.get(this.baseUrl + this.TotalMinoristaPendiente);
  }
  getTotalMinorista(): Observable<any> {
    return this.http.get(this.baseUrl + this.TotalMinorista);
  }
  getListarMinoristaporEstado(idestado: number): Observable<any> {
    return this.http.get(
      this.baseUrl + this.ListarMinoristaporEstado + '?Idestado=' + idestado
    );
  }

  getUltimoRegistroNroCotizacion(): Observable<any> {
    return this.http.get(this.baseUrl + this.UltimoRegistroNroCotizacion);
  }

  getObtenerMinorista(id: number): Observable<any> {
    return this.http.get(this.baseUrl + this.ObtenerMinorista + '/' + id);
  }

  getListarFuente(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarFuente);
  }

  getEliminarMinorista(quote: string): Observable<any> {
    return this.http.get(this.baseUrl + this.EliminarMinorista + quote);
  }
  getEliminarMinoristaDelete(quote: string): Observable<any> {
    return this.http.delete(
      this.baseUrl + this.EliminarMinoristaDelete + quote
    );
  }
  getObtenerUltimoRegistro(): Observable<any> {
    return this.http.get(this.baseUrl + this.ObtenerUltimoRegistro);
  }
  getListarEstadoMinorista(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarEstadoMinorista);
  }
  getRegistroMinoristaAll(formbody: any): Observable<any> {
    return this.http.post(this.baseUrl + this.RegistroMinoristaAll, formbody);
  }
  getDetailMinoristaAll(quote: string) {
    return this.http.get(this.baseUrl + this.DetailMinoristaAll + quote);
  }
}
