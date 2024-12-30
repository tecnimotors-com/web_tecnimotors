import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';

@Component({
  selector: 'app-homerepuesto',
  templateUrl: './homerepuesto.component.html',
  styleUrls: ['./homerepuesto.component.css'],
})
export class HomerepuestoComponent implements OnInit, OnDestroy {
  public ListtCategoriesCamera: any[] = [
    {
      tipo_articulo: 'MOTOCICLETA',
      descripcion_tipo_articulo: 'Motocicleta',
    },
    {
      tipo_articulo: 'BICICLETA',
      descripcion_tipo_articulo: 'Bicicleta',
    },
  ];
  public txtcategiesrepuesto: string = '';

  public ListTipoCategoria: any[] = [];
  public txttipocategoria: string = '';

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private router: Router
  ) {}

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

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.txtcategiesrepuesto = params['tipo'];
    });
    this.ListadoCategoria();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  limpiarbtn() {
    this.txttipocategoria = '';
  }

  ChangeCategoria() {
    this.router.navigate([`/homerepuesto/${this.txtcategiesrepuesto}`]);
    this.ListadoCategoria();
  }

  ListadoCategoria() {
    this.servicesmaestro
      .getListadoRepuestoTipoCategoria(this.txtcategiesrepuesto)
      .subscribe({
        next: (value: any) => {
          this.ListTipoCategoria = value;
        },
      });
  }

  ChangeTipoCategoria() {}
}
