import { Component, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { SharedMain } from '../../sharedmain';
import { environment } from '../../../../environments/environment.development';
import { MaestroclasificadoService } from '../../../core/service/maestroclasificado.service';
import { DepartamentoService } from '../../../core/service/departamento.service';
import { AuthService } from '../../../core/service/auth.service';
import Swal from 'sweetalert2';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-distribuidores',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './distribuidores.component.html',
  styleUrls: ['./distribuidores.component.scss'],
})
export class DistribuidoresComponent implements OnInit {
  public direccion: string = '';
  public direccion2: string = '';
  public urldireccion: SafeResourceUrl | undefined;

  public ListDepa: any[] = [];
  public ListProvi: any[] = [];
  public ListDistrito: any[] = [];
  public Lstdistribuidor: any[] = [];
  public txtid: string = '';
  public Iddepartamento: number = 0;
  public Idprovincia: number = 0;
  public Iddistrito: number = 0;
  public lbldepartamento: string = '';
  public lblprovincia: string = '';
  public lbldistrito: string = '';

  public txtnombre: string = '';
  public txtcelular: string = '';
  public txtdireccion: string = '';
  public txtdepa: string = '';
  public txtprovi: string = '';
  public txtdistri: string = '';

  constructor(
    private sanitizer: DomSanitizer,
    private maestroservice: MaestroclasificadoService,
    private departamento: DepartamentoService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();
    this.ListadoDistribuidores();
    this.ListadoDepartamento();
  }

  private setUrlDireccion(): void {
    const encodedDireccion = encodeURIComponent(this.direccion);
    const url = `https://www.google.com/maps/embed/v1/place?q=${encodedDireccion}&key=${environment.apikeygoogle}`;
    this.urldireccion = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

  ListadoDepartamento() {
    this.departamento.getListarDepartamentos().subscribe({
      next: (lst: any) => {
        this.ListDepa = lst;
      },
    });
  }

  ChangeDepartamento() {
    this.showAlert('Cargando información de distribuidores', 'info');
    const departamento$ = this.departamento.getObtenerDepartamento(
      this.Iddepartamento
    );
    const provincias$ = this.departamento.getListarProvincias(
      this.Iddepartamento
    );

    forkJoin([departamento$, provincias$]).subscribe({
      next: ([obter, listdistri]) => {
        const frombody = {
          departamento: obter.nombre.trim() ?? '',
          provincia: this.lblprovincia ?? '',
          distrito: this.lbldistrito ?? '',
        };

        this.maestroservice
          .getListadoGeneralDistribuidores(frombody)
          .subscribe({
            next: (dtl) => {
              this.Lstdistribuidor = dtl;
              this.lbldepartamento = obter.nombre.trim();
              this.ListProvi = listdistri;
              this.Idprovincia = 0;
              this.Iddistrito = 0;
              this.showAlert(
                'Se cargó exitosamente la información de distribuidores',
                'success'
              );
            },
            error: (err) => {
              console.error('Error al obtener distribuidores:', err);
              this.showAlert(
                'Error al cargar la información de distribuidores',
                'error'
              );
            },
          });
      },
      error: (err) => {
        console.error('Error al obtener departamento o provincias:', err);
        this.showAlert(
          'Error al cargar la información de departamento o provincias',
          'error'
        );
      },
    });
  }

  ListadoProvincia() {
    this.departamento.getListarProvincias(this.Iddepartamento).subscribe({
      next: (lstprovi) => {
        this.ListProvi = lstprovi;
      },
    });
  }

  ChangeProvincia() {
    this.showAlert('Cargando información de distribuidores', 'info');
    const Provincia$ = this.departamento.getObtenerProvincia(this.Idprovincia);
    const Distrito$ = this.departamento.getListarDistritos(
      this.Iddepartamento,
      this.Idprovincia
    );
    forkJoin([Provincia$, Distrito$]).subscribe({
      next: ([obter, listdistri]) => {
        const frombody = {
          departamento: this.lbldepartamento ?? '',
          provincia: obter.nombre.trim(),
          distrito: this.lbldistrito ?? '',
        };
        this.maestroservice
          .getListadoGeneralDistribuidores(frombody)
          .subscribe({
            next: (dtl) => {
              this.Lstdistribuidor = dtl;
              this.lblprovincia = obter.nombre.trim();
              this.ListDistrito = listdistri;
              this.Iddistrito = 0;
              this.showAlert(
                'Se cargó exitosamente la información de distribuidores',
                'success'
              );
            },
            error: (err) => {
              console.error('Error al obtener distribuidores:', err);
              this.showAlert(
                'Error al cargar la información de distribuidores',
                'error'
              );
            },
          });
      },
      error: (err) => {
        console.error('Error al obtener departamento o provincias:', err);
        this.showAlert(
          'Error al cargar la información de departamento o provincias',
          'error'
        );
      },
    });
  }

  ListadoDistrito() {
    this.departamento
      .getListarDistritos(this.Iddepartamento, this.Idprovincia)
      .subscribe({
        next: (lst: any) => {
          this.ListDistrito = lst;
        },
      });
  }

  ChangeDistrito() {
    this.showAlert('Cargando información de distribuidores', 'info');
    const Distrito$ = this.departamento.getObtenerDistrito(this.Iddistrito);
    forkJoin([Distrito$]).subscribe({
      next: ([obter]) => {
        const frombody = {
          departamento: this.lbldepartamento ?? '',
          provincia: this.lblprovincia,
          distrito: obter.nombre.trim(),
        };
        this.maestroservice
          .getListadoGeneralDistribuidores(frombody)
          .subscribe({
            next: (dtl) => {
              this.Lstdistribuidor = dtl;
              this.lbldistrito = obter.nombre.trim();
              this.showAlert(
                'Se cargó exitosamente la información de distribuidores',
                'success'
              );
            },
            error: (err) => {
              console.error('Error al obtener distribuidores:', err);
              this.showAlert(
                'Error al cargar la información de distribuidores',
                'error'
              );
            },
          });
      },
      error: (err) => {
        console.error('Error al obtener departamento o provincias:', err);
        this.showAlert(
          'Error al cargar la información de departamento o provincias',
          'error'
        );
      },
    });
  }

  ListadoDistribuidores() {
    this.maestroservice.getListadoDistribuidores().subscribe({
      next: (lst: any) => {
        this.Lstdistribuidor = lst;
        this.showAlert(
          'Se cargó exitosamente la información de distribuidores',
          'success'
        );
      },
      error: (err) => {
        console.error('Error al obtener distribuidores', err);
      },
    });
  }

  DetailDistribuidore() {
    const departamento$ = this.departamento.getObtenerDepartamento(
      this.Iddepartamento
    );
    const provincia$ = this.departamento.getObtenerProvincia(this.Idprovincia);
    const distrito$ = this.departamento.getObtenerDistrito(this.Iddistrito);

    this.clearDetalledistribuidore();
    this.showAlert('Buscando Información....', 'info');
    forkJoin([departamento$, provincia$, distrito$]).subscribe({
      next: ([resultdepa, resultprovi, resultdistrio]) => {
        const bodyform = this.buildBodyForm(
          resultdepa,
          resultprovi,
          resultdistrio
        );
        this.fetchListadoDetalleDistribuidore(bodyform);
      },
      error: (error) => {
        console.error('Error al obtener datos:', error);
      },
    });
  }

  private buildBodyForm(resultdepa: any, resultprovi: any, resultdistrio: any) {
    return {
      departamento: resultdepa.nombre ?? ''.trim().toUpperCase(),
      provincia: resultprovi.nombre ?? ''.trim().toUpperCase(),
      distrito: resultdistrio.nombre ?? ''.trim().toUpperCase(),
    };
  }

  private fetchListadoDetalleDistribuidore(bodyform: any) {
    this.maestroservice
      .getListadoDetalleDistribuidore(
        bodyform.departamento,
        bodyform.provincia,
        bodyform.distrito
      )
      .subscribe({
        next: (lst: any) => {
          if (Array.isArray(lst) && lst.length > 0) {
            this.txtid = '';
            this.Lstdistribuidor = lst;
            this.showAlert('Se encontraron resultados', 'success');
          } else {
            this.txtid = '';
            this.setUrlDireccion();
            this.clearDetalledistribuidore();

            this.showAlert('No Se encontraron resultados', 'info');
          }
        },
        error: (error) => {
          console.error('Error al obtener listado de distribuidores:', error);
        },
      });
  }
  clearDetalledistribuidore() {
    this.txtnombre = '';
    this.txtcelular = '';
    this.txtdireccion = '';
    this.txtdepa = '';
    this.txtprovi = '';
    this.txtdistri = '';
    this.txtid = '';
    this.direccion = '';
    this.direccion2 = '';
  }

  SelectFrom() {
    var id = this.txtid ?? '';
    if (id != '') {
      this.maestroservice
        .getDetailDistribuidores(parseInt(this.txtid))
        .subscribe({
          next: (dtl: any) => {
            this.direccion = dtl.direccion;
            this.direccion2 = dtl.direccion2;
            this.txtnombre = dtl.nombre;
            this.txtcelular = dtl.telefono;
            this.txtdireccion = dtl.direccion;
            this.txtdepa = dtl.departamento;
            this.txtprovi = dtl.provincia;
            this.txtdistri = dtl.distrito;
            this.setUrlDireccion();
          },
        });
    } else {
      this.txtid = '';
      this.direccion = '';
      this.direccion2 = '';
    }
  }

  // type AlertType = 'success' | 'error' | 'info';
  showAlert(texto: string, type: any) {
    Swal.fire({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 1500,
      timerProgressBar: true,
      title: texto,
      icon: type,
    });
  }

  ClearDistribuidores() {
    this.txtnombre = '';
    this.txtcelular = '';
    this.txtdireccion = '';
    this.direccion = '';
    this.direccion2 = '';
    this.txtdepa = '';
    this.txtprovi = '';
    this.txtdistri = '';
    this.txtid = '';
    this.direccion = '';
    this.direccion2 = '';
    this.setUrlDireccion();
  }

  ClearDireccion() {
    this.txtnombre = '';
    this.txtcelular = '';
    this.txtdireccion = '';
    this.direccion = '';
    this.direccion2 = '';
    this.txtdepa = '';
    this.txtprovi = '';
    this.txtdistri = '';
    this.txtid = '';
    this.direccion = '';
    this.direccion2 = '';
    this.setUrlDireccion();
  }

  ClearDepartamento() {
    this.showAlert('Cargando información de distribuidores', 'info');
    this.Iddepartamento = 0;
    this.Idprovincia = 0;
    this.Iddistrito = 0;
    this.lbldepartamento = '';
    this.lblprovincia = '';
    this.lbldistrito = '';
    this.ListDepa = [];
    this.ListProvi = [];
    this.ListDistrito = [];
    this.Lstdistribuidor = [];
    this.direccion = '';
    this.direccion2 = '';
    this.setUrlDireccion();
    this.ListadoDistribuidores();
    this.ListadoDepartamento();
  }

  ClearProvincia() {
    this.Idprovincia = 0;
    this.Iddistrito = 0;
    this.lblprovincia = '';
    this.lbldistrito = '';
    this.ListProvi = [];
    this.ListDistrito = [];
    this.Lstdistribuidor = [];
    this.direccion = '';
    this.direccion2 = '';
    this.setUrlDireccion();
    this.ListadoProvincia();
    this.ListgeneralDistribuidores();
  }

  ClearDistrito() {
    this.Iddistrito = 0;
    this.lbldistrito = '';
    this.ListDistrito = [];
    this.Lstdistribuidor = [];
    this.direccion = '';
    this.direccion2 = '';
    this.setUrlDireccion();
    this.ListadoDistrito();
    this.ListgeneralDistribuidores();
  }

  ListgeneralDistribuidores() {
    const frombody = {
      departamento: this.lbldepartamento ?? '',
      provincia: this.lblprovincia ?? '',
      distrito: this.lbldistrito ?? '',
    };
    this.maestroservice.getListadoGeneralDistribuidores(frombody).subscribe({
      next: (dtl) => {
        this.Lstdistribuidor = dtl;
      },
    });
  }
}
