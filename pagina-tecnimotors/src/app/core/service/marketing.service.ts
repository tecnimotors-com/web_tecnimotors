import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
@Injectable({
  providedIn: 'root',
})
export class MarketingService {
  private baseUrl = environment.myapimarketing;

  private registrarmayorista = '/Mayorista/Registromayoristacompleto';

  private ListarMayorista = '/Mayorista/ListarMayorista';

  private ListarMayoristaCompleto = '/Mayorista/ListarMayoristaCompleto';

  private TotalMayorista = '/Mayorista/TotalMayorista';
  private TotalMayoristaPendiente = '/Mayorista/TotalMayoristaPendiente';
  private TotalMayoristaAtendido = '/Mayorista/TotalMayoristaAtendido';
  private TotalMayoristafalse = '/Mayorista/TotalMayoristafalse';

  private ListarEstadoMayorista = '/EstadoMayorista/ListarEstadoMayorista';
  private ListarMayoristaporEstado = '/Mayorista/ListarMayoristaporEstado';

  private Registromayoristacompleto = '/Mayorista/Registromayoristacompleto';

  private UltimoRegistroMayorista = '/Mayorista/UltimoRegistroMayorista';

  private ListarFuente = '/Fuentes/ListarFuentes';

  private ListarDepartarmentos = '/Departamentos/ListarDepartamentos';

  private ListarProvincias = '/Provincias/ListarProvincias';

  private ListarDistritos = '/Distritos/ListarDistritos';

  private ObtenerMayorista = '/Mayorista/ObtenerMayorista';

  private actualizarmayorista = '/Mayorista/ActualizarMayorista';

  private actualizarMayoristaEstado = '/Mayorista/ActualizarMayoristaEstado';

  private DeleteMayorista = '/Mayorista/DeleteMayorista/';
  constructor(private http: HttpClient) {}

  getRegistrarmayorista(frombody: any): Observable<any> {
    return this.http.post(this.baseUrl + this.registrarmayorista, frombody);
  }

  getlistMayorista(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarMayorista);
  }
  getListarEstadoMayorista(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarEstadoMayorista);
  }

  getTotalMayorista(): Observable<any> {
    return this.http.get(this.baseUrl + this.TotalMayorista);
  }
  getTotalMayoristaPendiente(): Observable<any> {
    return this.http.get(this.baseUrl + this.TotalMayoristaPendiente);
  }
  getTotalMayoristaAtendido(): Observable<any> {
    return this.http.get(this.baseUrl + this.TotalMayoristaAtendido);
  }
  getTotalMayoristafalse(): Observable<any> {
    return this.http.get(this.baseUrl + this.TotalMayoristafalse);
  }

  getListarMayoristaporEstado(idestado: number): Observable<any> {
    return this.http.get(
      this.baseUrl + this.ListarMayoristaporEstado + '?Idestado=' + idestado
    );
  }

  getRegistromayoristacompleto(Mayorista: any): Observable<any> {
    return this.http.post(
      this.baseUrl + this.Registromayoristacompleto,
      Mayorista
    );
  }

  getObtenerUltimoRegistro(): Observable<any> {
    return this.http.get(this.baseUrl + this.UltimoRegistroMayorista);
  }
  getListarFuente(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarFuente);
  }
  getListardepartamentos(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarDepartarmentos);
  }
  getListarProvincias(departamento_id: number): Observable<any> {
    return this.http.get(
      this.baseUrl +
        this.ListarProvincias +
        '?departamento_id=' +
        departamento_id
    );
  }
  getListarDistritos(
    departamento_id: number,
    provincia_id: number
  ): Observable<any> {
    return this.http.get(
      this.baseUrl +
        this.ListarDistritos +
        '?departamento_id=' +
        departamento_id +
        '&provincia_id=' +
        provincia_id
    );
  }
  getObtenerMayorista(id: number): Observable<any> {
    return this.http.get(this.baseUrl + this.ObtenerMayorista + '/' + id);
  }

  getActualizarMayorista(Mayorista: any): Observable<any> {
    return this.http.put(this.baseUrl + this.actualizarmayorista, Mayorista);
  }

  getActualizarMayoristaEstado(mayorista: any): Observable<any> {
    return this.http.put(
      this.baseUrl + this.actualizarMayoristaEstado,
      mayorista
    );
  }

  getDeleteMayorista(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + this.DeleteMayorista + id);
  }
}
