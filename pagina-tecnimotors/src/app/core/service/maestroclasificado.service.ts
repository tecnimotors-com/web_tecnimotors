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

  constructor(private http: HttpClient) {}

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
}
