import { Component, OnDestroy, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { SharedMain } from '../../sharedmain';
import { environment } from '../../../../environments/environment.development';
import { MaestroclasificadoService } from '../../../core/service/maestroclasificado.service';
import { DepartamentoService } from '../../../core/service/departamento.service';
import { AuthService } from '../../../core/service/auth.service';
import Swal from 'sweetalert2';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';

@Component({
  selector: 'app-distribuidores',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './distribuidores.component.html',
  styleUrls: ['./distribuidores.component.scss'],
})
export class DistribuidoresComponent implements OnInit, OnDestroy {
  public direccion: string =
    'Av. Paseo de la Republica N° 1647. Distrito de la Victoria, Lima, Perú';
  public urldireccion: SafeResourceUrl | undefined;
  public direccion2: string =
    'Av.%20Paseo%20de%20la%20Republica%20N°%201647.%20Distrito%20de%20la%20Victoria,%20Lima,%20Perú';
  public ListDepa: any[] = [];
  public ListProvi: any[] = [];
  public ListDistrito: any[] = [];
  public Lstdistri: any[] = [];
  public txtid: string = '';
  public Iddepartamento: number = 0;
  public Idprovincia: number = 0;
  public Iddistrito: number = 0;

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
    this.setUrlDireccion();
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
        this.clearDetalledistribuidore();
      },
    });
  }

  ListadoProvincia() {
    this.departamento.getListarProvincias(this.Iddepartamento).subscribe({
      next: (lst: any) => {
        this.ListProvi = lst;
        this.Idprovincia = 0;
        this.Iddistrito = 0;
        this.clearDetalledistribuidore();
      },
    });
  }

  ListadoDistrito() {
    this.departamento
      .getListarDistritos(this.Iddepartamento, this.Idprovincia)
      .subscribe({
        next: (lst: any) => {
          this.ListDistrito = lst;
          this.Iddistrito = 0;
          this.clearDetalledistribuidore();
        },
      });
  }

  ListadoDistribuidores() {
    this.maestroservice.getListadoDistribuidores().subscribe({
      next: (lst: any) => {
        this.Lstdistri = lst;
      },
      error: (err) => {
        console.error('Error al obtener distribuidores', err);
      },
    });
  }

  ngOnDestroy(): void {}

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
          // Verifica si lst es un array y tiene elementos
          if (Array.isArray(lst) && lst.length > 0) {
            this.txtid = '';
            this.Lstdistri = lst;
            this.direccion =
              'Av. Paseo de la Republica N° 1647. Distrito de la Victoria, Lima, Perú';
            this.direccion2 =
              'Av.%20Paseo%20de%20la%20Republica%20N°%201647.%20Distrito%20de%20la%20Victoria,%20Lima,%20Perú';
            this.showAlert('Se encontraron resultados', 'success');
          } else {
            this.txtid = '';
            this.direccion =
              'Av. Paseo de la Republica N° 1647. Distrito de la Victoria, Lima, Perú';
            this.direccion2 =
              'Av.%20Paseo%20de%20la%20Republica%20N°%201647.%20Distrito%20de%20la%20Victoria,%20Lima,%20Perú';
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
    this.direccion =
      'Av. Paseo de la Republica N° 1647. Distrito de la Victoria, Lima, Perú';
    this.direccion2 =
      'Av.%20Paseo%20de%20la%20Republica%20N°%201647.%20Distrito%20de%20la%20Victoria,%20Lima,%20Perú';
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
      this.direccion =
        'Av. Paseo de la Republica N° 1647. Distrito de la Victoria, Lima, Perú';
      this.direccion2 =
        'Av.%20Paseo%20de%20la%20Republica%20N°%201647.%20Distrito%20de%20la%20Victoria,%20Lima,%20Perú';
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
}
