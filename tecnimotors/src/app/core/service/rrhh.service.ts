import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root',
})
export class RrhhService {
  private baseUrl = environment.myapiurlrrhh;

  private ColaboradorDetail = '/Colaborador/Obtenercolaboradorlogin';

  private Obtenercolaboradoruser = '/Colaborador/Obtenercolaboradoruser';

  private registrarToken = '/Auth/token';

  private RegistrarColaborador = '/Colaborador/RegistrarColaboradorfirstfase';
  private colaboradorRol = '/Colaborador/ListarColaboradorRol';
  private Detallelogin = '/Colaborador/Detallelogin';
  private ListaVendedorPublico = '/Colaborador/ListaVendedorPublico';
  private Obtenercolaboradorlogin = '/Colaborador/Obtenercolaboradorlogin';
  private listarContactoClienteMarketing =
    '/Colaborador/ListarContactoClienteMarketing';

  private ListarColaborador = '/Colaborador/ListarColaborador';
  private ListarUsuarioColaborador = '/Colaborador/ListarUsuarioColaborador';
  private ListarUsuarioColaboradordni =
    '/Colaborador/ListarUsuarioColaboradordni';

  //Sede
  private Listarsede = '/Sede/ListarSede';
  private Registrarsede = '/Sede/RegistrarSede';
  private Actualizarsede = '/Sede/ActualizarSede';
  private Detallesede = '/Sede/DetalleSede';

  //Area
  private ListarArea = '/Area/ListarArea';
  private RegistrarArea = '/Area/RegistrarArea';
  private ActualizarArea = '/Area/ActualizarArea';
  private DetalleArea = '/Area/DetalleArea';

  //Cargo
  private ListarCargo = '/Cargo/ListarCargo';
  private RegistrarCargo = '/Cargo/RegistrarCargo';
  private ActualizarCargo = '/Cargo/ActualizarCargo';
  private DetalleCargo = '/Cargo/DetalleCargo';

  //Tipo de documento
  private ListarTipoDocumento = '/TipoDocumento/ListarTipoDocumento';

  //Tipo de documento
  private ListarRol = '/Rol/ListarRol';

  //Tipo de documento
  private ListarGenero = '/Genero/ListarGenero';

  //Tipo de documento
  private ListarNacionalidad = '/Nacionalidad/ListarNacionalidad';

  private ListarEstadoColaborador =
    '/EstadoColaborador/ListarEstadoColaborador';

  constructor(private http: HttpClient) {}

  getcolaboradorRol(): Observable<any> {
    return this.http.get(environment.myapiurlrrhh + this.colaboradorRol);
  }
  getRegistrarRRHH(rrHHI: any): Observable<any> {
    return this.http.post(this.baseUrl + this.registrarToken, rrHHI);
  }
  getRegistrarColaborador(frombody: any) {
    return this.http.post(this.baseUrl + this.RegistrarColaborador, frombody);
  }
  getColaboradorDetail(dni: string): Observable<any> {
    return this.http.get(this.baseUrl + this.ColaboradorDetail + '/' + dni);
  }
  getObtenercolaboradoruser(user: string): Observable<any> {
    return this.http.get(
      this.baseUrl + this.Obtenercolaboradoruser + '/' + user
    );
  }
  getDetallelogin(user: string): Observable<any> {
    return this.http.get(this.baseUrl + this.Detallelogin + '/' + user);
  }
  getObtenercolaboradorlogin(user: string): Observable<any> {
    return this.http.get(
      this.baseUrl + this.Obtenercolaboradorlogin + '/' + user
    );
  }
  getlistarContactoClienteMarketing(): Observable<any> {
    return this.http.get(this.baseUrl + this.listarContactoClienteMarketing);
  }
  getListaVendedorPublico(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListaVendedorPublico);
  }
  getListarColaborador(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarColaborador);
  }
  getListarUsuarioColaborador(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarUsuarioColaborador);
  }
  getListarUsuarioColaboradordni(dnicola: string): Observable<any> {
    return this.http.get(
      this.baseUrl + this.ListarUsuarioColaboradordni + '/' + dnicola
    );
  }

  getListarTipoDocumento(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarTipoDocumento);
  }
  getListarArea(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarArea);
  }
  getListarCargo(idarea: number): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarCargo + '/' + idarea);
  }
  getListarsede(): Observable<any> {
    return this.http.get(this.baseUrl + this.Listarsede);
  }
  getListarGenero(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarGenero);
  }
  getListarEstadoColaborador(): Observable<any> {
    return this.http.get(this.baseUrl + this.ListarEstadoColaborador);
  }
}
