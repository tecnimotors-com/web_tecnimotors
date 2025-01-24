import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root',
})
export class RrhhService {
  private baseUrl = environment.myapiurlrrhh;

  private registrarToken = '/Auth/token';
  private ListaVendedorPublico = '/Colaborador/ListaVendedorPublico';

  private token = sessionStorage.getItem('tokenrrhh');
  private headertoken = new HttpHeaders({
    Authorization: `Bearer ${this.token}`,
  });

  constructor(private http: HttpClient) {}

  getListaVendedorPublico(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListaVendedorPublico, {
      headers: this.headertoken,
    });
  }
  getRegistrarRRHH(rrHHI: any): Observable<any> {
    return this.http.post(this.baseUrl + this.registrarToken, rrHHI);
  }
}
