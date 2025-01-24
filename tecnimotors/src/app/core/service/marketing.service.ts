import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.prod';
@Injectable({
  providedIn: 'root',
})
export class MarketingService {
  myurl = environment.myapimarketing;

  private registrarmayorista = '/Mayorista/Registromayoristacompleto';

  constructor(private http: HttpClient) {}

  getRegistrarmayorista(frombody: any): Observable<any> {
    return this.http.post(this.myurl + this.registrarmayorista, frombody);
  }
}
