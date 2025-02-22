import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class MaestroarticuloService {
  private myurl = environment.apimaestroarticulo;

  private ListArticulosGeneral = '/MaestroArticulo/ListArticulosGeneral';

  private AllfiltroPrincipalCocada = '/MaestroArticulo/AllfiltroPrincipalCocada';

  private DetalleArticulo = '/MaestroArticulo/DetalleArticulo/';

  /*--------------------------------------------------*/
  /*------------------- camara------------------------*/
  private ListCategorieCamara = '/MaestroArticulo/ListCategorieCamara';

  private ListModeloCamara = '/MaestroArticulo/ListModeloCamara/';

  private ListadoCamaraGeneral = '/MaestroArticulo/ListadoCamaraGeneral/';

  private DetalleCamaraAll = '/MaestroArticulo/DetalleCamaraAll/';

  private ListadoCamaraGeneralModelo = '/MaestroArticulo/ListadoCamaraGeneralModelo/';

  /*---------------------aceite-----------------------*/

  private ListCategorieAceite = '/MaestroArticulo/ListCategorieAceite';
  private ListModeloAceite = '/MaestroArticulo/ListModeloAceite/';
  private ListadoAceiteGeneral = '/MaestroArticulo/ListadoAceiteGeneral/';

  /*---------------------Vehiculos-----------------------*/

  private ListModeloVehiculo = '/MaestroArticulo/ListModeloVehiculo/';
  private ListadoVehiculoGeneral = '/MaestroArticulo/ListadoVehiculoGeneral/';

  /*---------------------Repuesto-----------------------*/

  private ListadoRepuestoTipoCategoria =
    '/MaestroArticulo/ListadoRepuestoTipoCategoria/';
  private ListadoModeloRepuesto = '/MaestroArticulo/ListadoModeloRepuesto/';
  private ListadoRepuestoGeneralALl = '/MaestroArticulo/ListadoRepuestoGeneralALl/';

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

  getListCategorieCamara(): Observable<any> {
    return this.http.get(this.myurl + this.ListCategorieCamara);
  }

  getListModeloCamara(cat: string, mar: string): Observable<any> {
    return this.http.get(this.myurl + this.ListModeloCamara + cat + '/' + mar);
  }

  getListadoCamaraGeneral(cat: string, mar: string): Observable<any> {
    return this.http.get(
      this.myurl + this.ListadoCamaraGeneral + cat + '/' + mar
    );
  }

  getDetalleCamaraAll(id: number): Observable<any> {
    return this.http.get(this.myurl + this.DetalleCamaraAll + id);
  }

  getListadoCamaraGeneralModelo(IdCamara: string): Observable<any> {
    return this.http.get(
      this.myurl + this.ListadoCamaraGeneralModelo + IdCamara
    );
  }

  getListCategorieAceite(): Observable<any> {
    return this.http.get(this.myurl + this.ListCategorieAceite);
  }

  getListModeloAceite(cat: string): Observable<any> {
    return this.http.get(this.myurl + this.ListModeloAceite + cat);
  }

  getListadoAceiteGeneral(cat: string): Observable<any> {
    return this.http.get(this.myurl + this.ListadoAceiteGeneral + cat);
  }

  getListModeloVehiculo(cat: string): Observable<any> {
    return this.http.get(this.myurl + this.ListModeloVehiculo + cat);
  }
  getListadoVehiculoGeneral(cat: string): Observable<any> {
    return this.http.get(this.myurl + this.ListadoVehiculoGeneral + cat);
  }

  getListadoRepuestoTipoCategoria(cat: string): Observable<any> {
    return this.http.get(this.myurl + this.ListadoRepuestoTipoCategoria + cat);
  }

  getListadoModeloRepuesto(tipocat: string, cat: string): Observable<any> {
    return this.http.get(
      this.myurl + this.ListadoModeloRepuesto + tipocat + '/' + cat
    );
  }

  getListadoRepuestoGeneralALl(cat: string): Observable<any> {
    return this.http.get(this.myurl + this.ListadoRepuestoGeneralALl + cat);
  }
}
