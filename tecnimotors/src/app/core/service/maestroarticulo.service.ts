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
  MaestroMarca = '/MaestroArticulo/GetMarca';
  AllSinFiltroArticulo = '/MaestroArticulo/GetAllSinFiltroArticulo';
  AllFiltroMarcaCocada = '/MaestroArticulo/GetAllFiltroMarcaCocada';

  constructor(private http: HttpClient) {}

  getMaestroArticulo(): Observable<any> {
    return this.http.get(this.myurl + this.MaestroArticulo);
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
}
