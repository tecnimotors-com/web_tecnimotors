import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ReclamoService {
  private baseUrl = environment.myapireclamo;

  private registroreclamo = '/Reclamo/RegistroReclamo';

  constructor(private http: HttpClient) {}

  getregistroreclamo(frombody: any): Observable<any> {
    return this.http.post(this.baseUrl + this.registroreclamo, frombody);
  }
}
