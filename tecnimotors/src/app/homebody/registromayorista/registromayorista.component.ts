import { DatePipe } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';

@Component({
  selector: 'app-registromayorista',
  templateUrl: './registromayorista.component.html',
  styleUrls: ['./registromayorista.component.css'],
})
export class RegistromayoristaComponent implements OnInit, OnDestroy {
  public fechregis: string = '';
  public myForm: FormGroup;
  public showAlert: boolean = true;

  constructor(
    private datePipe: DatePipe,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.myForm = this.fb.group({
      txtnombre: ['', Validators.required],
      txtapepater: ['', Validators.required],
      txtapemater: ['', Validators.required],
      txtcelular: ['', Validators.required],
      txttelefijo: [''],
      txtcorreo: ['', Validators.required],
      txtrazon: [''],
      txtruc: [''],
      txtdireccion: ['', Validators.required],
      txtdepa: [''],
      txtpro: [''],
      txtdis: [''],
      txtfecnac: ['', Validators.required],
      txtgenero: ['masculino', Validators.required],
      txtcoment: [''],
      txtcheck: [false],
    });
  }
  ngOnDestroy(): void {
    this.finalizePreLoader();
  }
  ngOnInit(): void {
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.fechregis = this.datePipe.transform(new Date(), 'yyyy-MM-dd')!;
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

  onSubmit() {
    if (this.myForm.valid) {
      this.fechregis = this.datePipe.transform(new Date(), 'yyyy-MM-dd')!;
      var form = this.myForm.value;
      const formulario: any = {
        nombre: form.txtnombre == '' ? '' : form.txtnombre.trim(),
        apellidopaterno: form.txtapepater == '' ? '' : form.txtapepater.trim(),
        apellidomaterno: form.txtapemater == '' ? '' : form.txtapemater.trim(),
        celular: form.txtcelular == '' ? '' : form.txtcelular.trim(),
        telefonofijo: form.txttelefijo == '' ? '' : form.txttelefijo.trim(),
        correo: form.txtcorreo == '' ? '' : form.txtcorreo.trim(),
        razonsocial: form.txtrazon == '' ? '' : form.txtrazon.trim(),
        ruc: form.txtruc == '' ? '' : form.txtruc.trim(),
        direccion: form.txtdireccion == '' ? '' : form.txtdireccion.trim(),
        comentario: form.txtcoment == '' ? '' : form.txtcoment.trim(),
        genero: form.txtgenero == '' ? '' : form.txtgenero.trim(),
        fechanacimiento: form.txtfecnac == '' ? '' : form.txtfecnac.trim(),
        fecharegistro: this.fechregis,
        protecciondatos: form.txtcheck,
        departamento_id: 0,
        provincia_id: 0,
        distrito_id: 0,
        fuente_id: 1,
        estado_id: 2,
      };
      this.showAlert = true;
      //this.showAlert = true;
      console.log(formulario);
    } else {
      console.log('Formulario no válido');
      // Verificar qué controles son inválidos
      Object.keys(this.myForm.controls).forEach((controlName) => {
        const control = this.myForm.get(controlName);
        if (control && control.invalid) {
          console.log(`${controlName} es inválido`, control.errors);
        }
        //controlName == 'txtnombre'? this.showAlert == false: this.showAlert == true;
      });
      //this.myForm.get('txtnombre')
      //this.showAlert = false;
    }
  }

  onInputChange() {
    this.cdr.markForCheck(); // Marca el componente para la detección de cambios
  }
}
