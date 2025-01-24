import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.prod';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DepartamentoService {
  myurl = environment.myapiurldepa;

  private registrarToken = '/Auth/token';
  private ListarDepartamentos = '/Departamento/ListarDepartamentos';
  private ListarProvincias = '/Provincias/ListarProvincias/?departamento_id=';
  private ListarDistritos = '/Distritos/ListarDistritos?departamento_id=';
  private detalledepartamento =
    '/Departamento/ObtenerDepartamento?Departamento_id=';
  private detalleprovincia = '/Provincias/ObtenerProvincia/';
  private detalledistrito = '/Distritos/ObtenerDistrito/';

  private token = sessionStorage.getItem('tokendepa');
  private headertoken = new HttpHeaders({
    Authorization: `Bearer ${this.token}`,
  });

  constructor(private http: HttpClient) {}

  getRegistrarDepartamento(departamento: any): Observable<any> {
    return this.http.post(this.myurl + this.registrarToken, departamento);
  }

  getListarDepartamentos(): Observable<any> {
    return this.http.get(this.myurl + this.ListarDepartamentos, {
      headers: this.headertoken,
    });
  }

  getListarProvincias(depaid: number): Observable<any> {
    return this.http.get(this.myurl + this.ListarProvincias + depaid, {
      headers: this.headertoken,
    });
  }

  getListarDistritos(depaid: number, proviid: number): Observable<any> {
    return this.http.get(
      this.myurl + this.ListarDistritos + depaid + '&provincia_id=' + proviid,
      {
        headers: this.headertoken,
      }
    );
  }

  // -------------------- Consultar Departamentos ------------------------
  getdetalledepartamento(id: string): Observable<any> {
    return this.http.get(this.myurl + this.detalledepartamento + id, {
      headers: this.headertoken,
    });
  }

  getdetalleprovincia(id: string): Observable<any> {
    return this.http.get(this.myurl + this.detalleprovincia + id, {
      headers: this.headertoken,
    });
  }

  getdetalledistrito(id: string): Observable<any> {
    return this.http.get(this.myurl + this.detalledistrito + id, {
      headers: this.headertoken,
    });
  }
}
