import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class MaestroarticuloService {
  myurl = environment.apimaestroarticulo;

  ListArticulosGeneral = '/MaestroArticulo/ListArticulosGeneral';

  AllfiltroPrincipalCocada = '/MaestroArticulo/AllfiltroPrincipalCocada';

  DetalleArticulo = '/MaestroArticulo/DetalleArticulo/';

  /*--------------------------------------------------*/

  constructor(private http: HttpClient) {}

  getListArticulosGeneral(): Observable<any> {
    return this.http.get(this.myurl + this.ListArticulosGeneral);
  }

  /*-----------------------------------------------------------------------------*/
  // ALL COCADA
  getAllfiltroPrincipalCocada(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.AllfiltroPrincipalCocada, frombody);
  }

  getDetalleArticulo(id: number): Observable<any> {
    return this.http.get(this.myurl + this.DetalleArticulo + id);
  }
}
