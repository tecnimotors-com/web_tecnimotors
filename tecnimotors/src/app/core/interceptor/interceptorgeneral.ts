import {
  HttpClient,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';

import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.prod';

@Injectable()
export class Interceptorgeneral implements HttpInterceptor {
  static acestokenalmacen = '';
  constructor(private http: HttpClient) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const tokenrrhh = sessionStorage.getItem('tokenrrhh')!;

    if (tokenrrhh) {
      const realease = request.clone({
        setHeaders: {
          Authorization: request.urlWithParams.startsWith(
            environment.myapiurlrrhh
          )
            ? `Bearer ${tokenrrhh}`
            : `Bearer ${tokenrrhh}`,
        },
      });
      return next.handle(realease);
    }
    return next.handle(request);
  }
}
