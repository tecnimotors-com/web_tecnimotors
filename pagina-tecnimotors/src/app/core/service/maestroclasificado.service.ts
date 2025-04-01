import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MaestroclasificadoService {
  private myurl = environment.apimaestroarticulo;

  private ListadoCategoriaVehiculos =
    '/MaestroClasificado/ListadoCategoriaVehiculos';

  private ListadoGeneralCategoria =
    '/MaestroClasificado/ListadoGeneralCategoria/';

  private ListadoGeneralVehiculos =
    '/MaestroClasificado/ListadoGeneralVehiculos';

  private ListadoModeloVehiculo = '/MaestroClasificado/ListadoModeloVehiculo';

  private ListarMarcaVehiculo = '/MaestroClasificado/ListarMarcaVehiculo';

  private DetalleVehiculo = '/MaestroClasificado/DetalleVehiculo/';

  private ListadoDistribuidores = '/Distribuidores/ListadoDistribuidores';

  private DetailDistribuidores = '/Distribuidores/DetailDistribuidores/';

  private ListadoDetalleDistribuidore =
    '/Distribuidores/ListadoDetalleDistribuidore/';

  private TipoMarcaAceite = '/MaestroClasificado/TipoMarcaAceite';
  private ListadoGeneralAceite = '/MaestroClasificado/ListadoGeneralAceite';

  private ListadoRepuestoCategoria =
    '/MaestroClasificado/ListadoRepuestoCategoria';
  private ListadoRepuestoMarca = '/MaestroClasificado/ListadoRepuestoMarca';
  private ListadoGeneralRepuesto = '/MaestroClasificado/ListadoGeneralRepuesto';

  private ListadoTipoCamaras = '/MaestroClasificado/ListadoTipoCamaras';
  private ListadoCamaraMarca = '/MaestroClasificado/ListadoCamaraMarca';
  private ListadoGeneralCamara = '/MaestroClasificado/ListadoGeneralCamara';

  private AllfiltroPrincipalCocada =
    '/MaestroClasificado/AllfiltroPrincipalLLanta';
  private ListadoAnchoPerfil = '/MaestroClasificado/ListadoAnchoPerfilLLANTA';

  private ListadoLLantaMedida = '/MaestroClasificado/ListadoLLantaMedida';
  private ListadoLLantaModelo = '/MaestroClasificado/ListadoLLantaModelo';
  private ListadoLLantaMarca = '/MaestroClasificado/ListadoLLantaMarca';
  private ListadoLLantaCategoria = '/MaestroClasificado/ListadoLLantaCategoria';

  constructor(private http: HttpClient) {}

  getListadoAnchoPerfil(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoAnchoPerfil);
  }

  getAllfiltroPrincipalCocada(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.AllfiltroPrincipalCocada, frombody);
  }

  getListadoCategoriaVehiculos(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoCategoriaVehiculos);
  }

  getListadoGeneralCategoria(catego: string): Observable<any> {
    return this.http.get(this.myurl + this.ListadoGeneralCategoria + catego);
  }

  getListadoGeneralVehiculos(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoGeneralVehiculos, frombody);
  }

  getListadoModeloVehiculo(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoModeloVehiculo, frombody);
  }
  getListarMarcaVehiculo(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.ListarMarcaVehiculo, frombody);
  }

  getDetalleVehiculo(Id: number): Observable<any> {
    return this.http.get(this.myurl + this.DetalleVehiculo + Id);
  }

  getListadoDistribuidores(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoDistribuidores);
  }

  getDetailDistribuidores(Id: number): Observable<any> {
    return this.http.get(this.myurl + this.DetailDistribuidores + Id);
  }

  getListadoDetalleDistribuidore(
    depa: string,
    provi: string,
    distri: string
  ): Observable<any> {
    const url = `${this.myurl}${this.ListadoDetalleDistribuidore}${depa}/${provi}/${distri}`;
    return this.http.get(url);
  }

  getTipoMarcaAceite(): Observable<any> {
    return this.http.get(this.myurl + this.TipoMarcaAceite);
  }

  getListadoGeneralAceite(dtl: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoGeneralAceite, dtl);
  }

  getListadoRepuestoCategoria(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoRepuestoCategoria);
  }
  getListadoRepuestoMarca(from: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoRepuestoMarca, from);
  }
  getListadoGeneralRepuesto(from: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoGeneralRepuesto, from);
  }

  getListadoTipoCamaras(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoTipoCamaras);
  }
  getListadoCamaraMarca(from: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoCamaraMarca, from);
  }
  getListadoGeneralCamara(from: any): Observable<any> {
    return this.http.post(this.myurl + this.ListadoGeneralCamara, from);
  }

  getListadoLLantaMedida(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoLLantaMedida);
  }
  getListadoLLantaModelo(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoLLantaModelo);
  }
  getListadoLLantaMarca(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoLLantaMarca);
  }
  getListadoLLantaCategoria(): Observable<any> {
    return this.http.get(this.myurl + this.ListadoLLantaCategoria);
  }
}
