import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaestroarticuloService } from '../../../core/service/maestroarticulo.service';

@Component({
  selector: 'app-detallevehiculo',
  templateUrl: './detallevehiculo.component.html',
  styleUrls: ['./detallevehiculo.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class DetallevehiculoComponent implements OnInit, OnDestroy {
  public srcimg: string = '';

  public id: number = 0;

  public Dtlid: number = 0;
  public Dtlcodigoimg: string = '';
  public Dtlcodigo: string = '';
  public DtlDescripcion: string = '';
  public DtlFamilia: string = '';
  public DtlSubFamilia: string = '';
  public DtlMarca: string = '';
  public DtlTipoArticulo: string = '';
  public DtlUnidadMedida: string = '';

  public count: number = 1;
  public blndisable = false;

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService
  ) {}

  ngOnInit(): void {
    this.DetailArticulo();

    setTimeout(() => {
      window.scrollTo(0, 0);
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

  DetailArticulo() {
    this.route.params.subscribe((params) => {
      this.id = params['id'];

      this.servicesmaestro.getDetalleCamaraAll(this.id).subscribe({
        next: (dtl: any) => {
          this.Dtlid = dtl.id;
          this.Dtlcodigoimg = dtl.codigo;
          this.Dtlcodigo = dtl.codigo.slice(-5);
          this.DtlDescripcion = dtl.descripcion;
          this.DtlFamilia = dtl.familia;
          this.DtlSubFamilia = dtl.subfamilia;
          this.DtlMarca = dtl.marca;
          this.DtlTipoArticulo = dtl.descripcion_tipo_articulo;
          this.DtlUnidadMedida = dtl.unidad_medida;

          this.srcimg =
            '../../../../assets/img/product/big-product/product1.webp';
        },
      });
    });
  }

  DtlImagen1() {
    this.srcimg = '../../../../assets/img/product/big-product/product1.webp';
  }

  DtlImagen2() {
    this.srcimg = '../../../../assets/img/product/main-product/product2.webp';
  }

  DtlImagen3() {
    this.srcimg = '../../../../assets/img/product/main-product/product5.webp';
  }

  AumentarCount() {
    if (this.count < 10) {
      this.blndisable = false;
      this.count++;
    } else {
      this.blndisable = true;
      this.count = 10;
    }
  }

  RestarCount() {
    if (this.count <= 1) {
      this.count = 1;
    } else {
      this.count--;
    }
  }
}
