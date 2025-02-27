import { Component, OnDestroy, OnInit } from '@angular/core';
import { SharedMain } from '../../sharedmain';
import { DatePipe } from '@angular/common';
import { ReclamoService } from '../../../core/service/reclamo.service';
import Swal from 'sweetalert2';
import { AuthService } from '../../../core/service/auth.service';

@Component({
  selector: 'app-libroreclamo',
  imports: [SharedMain],
  templateUrl: './libroreclamo.component.html',
  styleUrls: ['./libroreclamo.component.scss'],
})
export class LibroreclamoComponent implements OnInit, OnDestroy {
  public tipodoc: string = '';
  public ndocumento: string = '';
  public nombre: string = '';
  public apellidoparteno: string = '';

  public apellidomaterno: string = '';
  public telefono: string = '';
  public correo: string = '';
  public direccion: string = '';

  public fechasolicitud: string = '';
  public tiporeclamo: string = '';
  public motivo: string = '';
  public detallereclamo: string = '';
  public estado: string = 'Pendiente';

  constructor(
    private datePipe: DatePipe,
    private reclamoservice: ReclamoService,private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();
    this.fechasolicitud = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;
    setTimeout(() => {
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
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

  btnEnviar() {
    this.fechasolicitud = this.datePipe.transform(new Date(), 'dd/MM/yyyy')!;

    const frombody = {
      tipodoc: this.tipodoc,
      ndocumento: this.ndocumento,
      nombre: this.nombre,
      apellidoparteno: this.apellidoparteno,
      apellidomaterno: this.apellidomaterno,
      telefono: this.telefono,
      correo: this.correo,
      direccion: this.direccion,
      fechasolicitud: this.fechasolicitud,
      tiporeclamo: this.tiporeclamo,
      motivo: this.motivo,
      detallereclamo: this.detallereclamo,
      estado: this.estado,
    };

    this.reclamoservice.getregistroreclamo(frombody).subscribe({
      next: () => {
        this.fechasolicitud = this.datePipe.transform(
          new Date(),
          'dd/MM/yyyy'
        )!;
        this.tipodoc = '';
        this.ndocumento = '';
        this.nombre = '';
        this.apellidoparteno = '';
        this.apellidomaterno = '';
        this.telefono = '';
        this.correo = '';
        this.direccion = '';
        this.tiporeclamo = '';
        this.motivo = '';
        this.detallereclamo = '';
        this.estado = '';
        this.showAlert('Se Registro correctamente la información', 'success');
      },
      error: () => {},
    });
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
