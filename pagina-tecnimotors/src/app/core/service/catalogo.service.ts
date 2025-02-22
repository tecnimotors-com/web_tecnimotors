import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class CatalogoService {
  private baseUrl = environment.myapiurlcatalogo;

  private listafiltrocatalogo = '/Catalogo/ListarCatalogo';
  private listarcatalogocompleto = '/Catalogo/ListarCatalogoCompleto';
  private obtenerCatalogo = '/Catalogo/ObtenerCatalogo';
  private ListarTipoCatalogo = '/TipoCatalogo/ListarTipoCatalogo';
  private ListarFiltroTipoCatalogo = '/Catalogo/ListarFiltroTipoCatalogo?Id=';

  constructor(private http: HttpClient) {}

  getListarCatalogoCompleto(): Observable<any> {
    return this.http.get(this.baseUrl + this.listarcatalogocompleto);
  }

  getobtenerCatalogo(id: number): Observable<any> {
    return this.http.get(this.baseUrl + this.obtenerCatalogo + '/' + id);
  }

  getlistafiltrocatalogo(): Observable<any> {
    return this.http.get(this.baseUrl + this.listafiltrocatalogo);
  }

  getListarTipoCatalogo(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarTipoCatalogo);
  }
  getListarFiltroTipoCatalogo(id: number): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarFiltroTipoCatalogo + id);
  }
}
