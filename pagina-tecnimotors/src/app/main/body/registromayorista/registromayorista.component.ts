import { SharedMain } from '../../sharedmain';
import { DatePipe } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { AuthService } from '../../../core/service/auth.service';
import { DepartamentoService } from '../../../core/service/departamento.service';
import { MarketingService } from '../../../core/service/marketing.service';

@Component({
  selector: 'app-registromayorista',
  imports: [SharedMain],
  templateUrl: './registromayorista.component.html',
  styleUrls: ['./registromayorista.component.scss'],
})
export class RegistromayoristaComponent implements OnInit, OnDestroy {
  public fechregis: string = '';
  public showAlert: boolean = true;

  /*----------registrar mayorista---------------*/
  public lbnombre: string = '';
  public lbapellidoparteno: string = '';
  public lbapellidomaterno: string = '';
  public lbcelular: string = '';
  public lbtelefijo: string = '';
  public lbcorreo: string = '';
  public lbrucdni: string = '';
  public lbrazonnombre: string = '';
  public lbdireccion: string = '';
  public lbldepartamento: string = '';
  public lblprovincia: string = '';
  public lbldistrito: string = '';
  public lbfechanac: string = '';
  public lbgenero: string = 'masculino';
  public lbcoment: string = '';
  public lbcheckauto: boolean = false;

  public departamento_id: number = 0;
  public provincia_id: number = 0;
  public distrito_id: number = 0;
  public listDepartamento: any[] = [];
  public listProvincia: any[] = [];
  public listDistrito: any[] = [];
  public nombredepartamento: string = '';
  public nombredistrito: string = '';
  public nombreprovincia: string = '';

  /*--------------------------------------*/
  public alerts: any = {
    showalertnom: false,
    showalertpater: false,
    showalertmater: false,
    showalertcelular: false,
    showalertcorreo: false,
    showalertdireccion: false,
  };
  public showsuccess: boolean = false;

  constructor(
    private datePipe: DatePipe,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private auth: AuthService,
    private departamentoservice: DepartamentoService,
    private marketingservice: MarketingService
  ) {}

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  ngOnInit(): void {
    this.auth.getRefreshToken();
    this.ListadoDepartamento();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.fechregis = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  private initializePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');

    if (preloaderWrapper) {
      preloaderWrapper.classList.remove('loaded');

      setTimeout(() => {
        preloaderWrapper.classList.add('loaded');
      }, 1000);
    } else {
      console.error('Preloader not found!');
    }
  }

  private finalizePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');
    if (preloaderWrapper) {
      preloaderWrapper.classList.add('loaded');
    }
  }

  onInputChange() {
    this.cdr.markForCheck();
  }

  ListadoDepartamento() {
    this.departamentoservice.getListarDepartamentos().subscribe({
      next: (value: any) => {
        this.listDepartamento = value;
      },
    });
  }

  ObtenerProvincia() {
    if (this.departamento_id == null) {
      this.listDistrito = [];
      this.listProvincia = [];
      this.provincia_id = 0;
      this.distrito_id = 0;
    } else if (this.departamento_id == 0) {
      this.departamentoservice.getListarProvincias(0).subscribe({
        next: (value: any) => {
          this.listProvincia = value;
        },
      });
      this.departamentoservice.getListarDistritos(0, 0).subscribe({
        next: (value) => {
          this.listDistrito = value;
        },
      });
    } else if (this.departamento_id != 0) {
      this.departamentoservice
        .getListarProvincias(this.departamento_id)
        .subscribe({
          next: (value) => {
            this.listProvincia = value;
          },
        });
    }
  }
  listarDistrito() {
    if (this.provincia_id == null) {
      this.listDistrito = [];
      this.distrito_id = 0;
    } else if (this.departamento_id == 0 && this.provincia_id == 0) {
      this.departamentoservice.getListarDistritos(0, 0).subscribe({
        next: (value) => {
          this.listDistrito = value;
        },
      });
    } else if (this.departamento_id != 0 && this.provincia_id != 0) {
      this.departamentoservice
        .getListarDistritos(this.departamento_id, this.provincia_id)
        .subscribe({
          next: (value) => {
            this.listDistrito = value;
          },
        });
    }
  }

  getdistrito() {
    this.departamentoservice
      .getdetalledepartamento('' + this.departamento_id)
      .subscribe({
        next: (value) => {
          this.nombredepartamento = value.nombre;
          this.lbldepartamento = this.nombredepartamento;
        },
      });

    this.departamentoservice
      .getdetalleprovincia('' + this.provincia_id)
      .subscribe({
        next: (value) => {
          this.nombreprovincia = value.nombre;
          this.lblprovincia = this.nombreprovincia;
        },
      });

    this.departamentoservice
      .getdetalledistrito('' + this.distrito_id)
      .subscribe({
        next: (value) => {
          if (value == null) {
            this.nombredistrito = '';
          } else {
            this.nombredistrito = value.nombre;
            this.lbldistrito = this.nombredistrito;
          }
        },
      });
  }
  /*-------------------------------------*/
  /*
  Registrarse() {
    this.fechregis = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;
    if (this.lbnombre == '') {
      this.showalertnom = true;
    } else if (this.lbapellidoparteno == '') {
      this.showalertpater = true;
    } else if (this.lbapellidomaterno == '') {
      this.showalertmater = true;
    } else if (this.lbcelular == '') {
      this.showalertcelular = true;
    } else if (this.lbcorreo == '') {
      this.showalertcorreo = true;
    } else if (this.lbdireccion == '') {
      this.showalertdireccion = true;
    } else {
      const formulario: any = {
        nombre: this.lbnombre == '' ? '' : this.lbnombre.trim(),
        apellidopaterno:
          this.lbapellidoparteno == '' ? '' : this.lbapellidoparteno.trim(),
        apellidomaterno:
          this.lbapellidomaterno == '' ? '' : this.lbapellidomaterno.trim(),
        celular: this.lbcelular == '' ? '' : this.lbcelular.trim(),
        telefonofijo: this.lbtelefijo == '' ? '' : this.lbtelefijo.trim(),
        correo: this.lbcorreo == '' ? '' : this.lbcorreo.trim(),
        razonsocial: this.lbrazonnombre == '' ? '' : this.lbrazonnombre.trim(),
        ruc: this.lbrucdni == '' ? '' : this.lbrucdni.trim(),
        direccion: this.lbdireccion == '' ? '' : this.lbdireccion.trim(),
        comentario: this.lbcoment == '' ? '' : this.lbcoment.trim(),
        genero: this.lbgenero == '' ? '' : this.lbgenero.toString().trim(),
        fechanacimiento: this.lbfechanac == '' ? '' : this.lbfechanac.trim(),
        fecharegistro: this.fechregis,
        protecciondatos: this.lbcheckauto.toString(),
        departamento_id: this.departamento_id,
        provincia_id: this.provincia_id,
        distrito_id: this.distrito_id,
        fuente_id: 1,
        estado_id: 2,
      };
      this.showAlert = false;
      //this.showAlert = true;
    }
  }
  */

  Registrarse() {
    this.fechregis = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;

    this.resetAlerts();

    if (this.validateFields()) {
      const formulario = this.buildForm();
      this.showAlert = false;
      this.showsuccess = true;

      this.marketingservice.getRegistrarmayorista(formulario).subscribe({
        next: () => {
          this.limpiarlabel();
          setTimeout(() => {
            this.showsuccess = false; // Oculta la alerta
          }, 8000);
        },
      });
    }
  }

  limpiarlabel() {
    this.lbnombre = '';
    this.lbapellidoparteno = '';
    this.lbapellidomaterno = '';
    this.lbcelular = '';
    this.lbtelefijo = '';
    this.lbcorreo = '';
    this.lbrucdni = '';
    this.lbrazonnombre = '';
    this.lbdireccion = '';
    this.lbldepartamento = '';
    this.lblprovincia = '';
    this.lbldistrito = '';
    this.lbfechanac = '';
    this.lbgenero = 'masculino';
    this.lbcoment = '';
    this.lbcheckauto = false;
    this.departamento_id = 0;
    this.provincia_id = 0;
    this.distrito_id = 0;
    this.listProvincia = [];
    this.listDistrito = [];
  }

  private resetAlerts() {
    for (const key in this.alerts) {
      if (this.alerts.hasOwnProperty(key)) {
        this.alerts[key] = false;
      }
    }
  }

  private validateFields(): boolean {
    let isValid = true;

    if (this.lbnombre.trim() === '') {
      this.alerts.showalertnom = true;
      isValid = false;
    }
    if (this.lbapellidoparteno.trim() === '') {
      this.alerts.showalertpater = true;
      isValid = false;
    }
    if (this.lbapellidomaterno.trim() === '') {
      this.alerts.showalertmater = true;
      isValid = false;
    }
    if (this.lbcelular.trim() === '') {
      this.alerts.showalertcelular = true;
      isValid = false;
    }
    if (this.lbcorreo.trim() === '') {
      this.alerts.showalertcorreo = true;
      isValid = false;
    }
    if (this.lbdireccion.trim() === '') {
      this.alerts.showalertdireccion = true;
      isValid = false;
    }

    return isValid;
  }

  private buildForm() {
    return {
      nombre: this.lbnombre.trim(),
      apellidopaterno: this.lbapellidoparteno.trim(),
      apellidomaterno: this.lbapellidomaterno.trim(),
      celular: this.lbcelular.trim(),
      telefonofijo: this.lbtelefijo.trim(),
      correo: this.lbcorreo.trim(),
      razonsocial: this.lbrazonnombre.trim(),
      ruc: this.lbrucdni.trim(),
      direccion: this.lbdireccion.trim(),
      comentario: this.lbcoment.trim(),
      genero: this.lbgenero.toString().trim(),
      fechanacimiento: this.lbfechanac.trim(),
      fecharegistro: this.fechregis,
      protecciondatos: this.lbcheckauto.toString(),
      departamento_id: this.departamento_id,
      provincia_id: this.provincia_id,
      distrito_id: this.distrito_id,
      fuente_id: 1,
      estado_id: 1,
    };
  }
}
