import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class MaestroarticuloService {
  myurl = environment.apimaestroarticulo;

  MaestroArticulo = '/MaestroArticulo/GetArticulos';
  MaestroArticuloAsync = '/MaestroArticulo/GetArticulosAsync';
  MaestroPrincipalAsync = '/MaestroArticulo/GetPrincipalAsync';

  FiltroPrincipal = '/MaestroArticulo/FiltroPrincipal';
  FiltroPrincipalAro = '/MaestroArticulo/FiltroPrincipalAro';
  FiltroPrincipalCocada = '/MaestroArticulo/FiltroPrincipalCocada';
  FiltroPrincipalMarca = '/MaestroArticulo/FiltroPrincipalMarca';
  FiltroPrincipalTU = '/MaestroArticulo/FiltroPrincipalTU';

  /*--------------------------------------------------*/
  //ALL COCADA
  AllfiltroPrincipalCocada = '/MaestroArticulo/AllfiltroPrincipalCocada';

  /*--------------------------------------------------*/
  MaestroMarca = '/MaestroArticulo/GetMarca';
  AllSinFiltroArticulo = '/MaestroArticulo/GetAllSinFiltroArticulo';
  AllFiltroMarcaCocada = '/MaestroArticulo/GetAllFiltroMarcaCocada';

  constructor(private http: HttpClient) {}

  getMaestroArticulo(): Observable<any> {
    return this.http.get(this.myurl + this.MaestroArticulo);
  }

  getMaestroArticuloAsync(): Observable<any> {
    return this.http.get(
      this.myurl + this.MaestroArticuloAsync 
    );
  }

  getMaestroPrincipalAsync(limit: number, offset: number): Observable<any> {
    return this.http.get(
      this.myurl + this.MaestroPrincipalAsync + '/' + limit + '/' + offset
    );
  }

  getFiltroPrincipal(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.FiltroPrincipal, frombody);
  }

  getFiltroPrincipalAro(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.FiltroPrincipalAro, frombody);
  }

  getFiltroPrincipalCocada(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.FiltroPrincipalCocada, frombody);
  }

  getFiltroPrincipalMarca(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.FiltroPrincipalMarca, frombody);
  }

  getFiltroPrincipalTU(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.FiltroPrincipalTU, frombody);
  }

  getMaestroMarca(): Observable<any> {
    return this.http.get(this.myurl + this.MaestroMarca);
  }

  getAllSinFiltroArticulo(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.AllSinFiltroArticulo, frombody);
  }

  getAllFiltroMarcaCocada(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.AllFiltroMarcaCocada, frombody);
  }

  /*-----------------------------------------------------------------------------*/
  // ALL COCADA
  getAllfiltroPrincipalCocada(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.AllfiltroPrincipalCocada, frombody);
  }
}
