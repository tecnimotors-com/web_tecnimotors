import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { SharedMain } from '../../../sharedmain';
import { MaestroarticuloService } from '../../../../core/service/maestroarticulo.service';
import { CotizacionService } from '../../../../core/service/cotizacion.service';
import { AuthService } from '../../../../core/service/auth.service';
import { MaestroclasificadoService } from '../../../../core/service/maestroclasificado.service';
import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-detallevehiculo',
  imports: [SharedMain],
  templateUrl: './detallevehiculo.component.html',
  styleUrls: ['./detallevehiculo.component.scss'],
})
export class DetallevehiculoComponent implements OnInit, OnDestroy {
  
  public srcimg: string =
    '../../../../assets/img/product/big-product/product1.webp';
  public lbldescripcion: string = '';
  public lblunidadmedida: string = '';
  public lblcategoria: string = '';
  public lblmarca: string = '';
  public lblmarcaoriginal: string = '';
  public lblaplicacion: string = '';
  public lblproducto: string = '';
  public lblmedida: string = '';
  public lblmodelo: string = '';
  public lblmedidaestandarizado: string = '';
  public lbltipo1: string = '';
  public lblid: number = 0;
  public lblcodigo: string = '';
  public lblfamilia: string = '';
  public lblsubfamilia: string = '';
  public lbltipo: string = '';
  public lblcodigoequivalente: string = '';
  public lblpathoriginal: string = '';
  public lbltotalImagenes: number = 0;

  public count: number = 1;
  public blndisable = false;
  public txtacuerdotrue: boolean = false;
  private cartSubscription!: Subscription | undefined;
  public ListCarrito: any[] = [];
  public isVisible: boolean = false;
  public txtlink: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public txtlinkoriginal: string =
    environment.apimaestroarticulo + '/MaestroClasificado/GetBanner2?ruta=';
  public Listrutaoriginal: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private servicesmaestro: MaestroarticuloService,
    private auth: AuthService,
    private cotizacionService: CotizacionService,
    private serviceclasificado: MaestroclasificadoService
  ) {}

  ngOnInit(): void {
    this.DetalleVehiculo();

    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items; // Actualiza la lista de productos en el carrito
    });
    setTimeout(() => {
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  DetalleVehiculo() {
    this.route.params.subscribe((params) => {
      this.lblid = parseInt(params['id']);

      this.serviceclasificado.getDetalleVehiculo(this.lblid).subscribe({
        next: (dtl: any) => {
          console.log(dtl);
          this.lbldescripcion = dtl.listado.descripcion;
          this.lblunidadmedida = dtl.listado.unidadmedida;
          this.lblcategoria = dtl.listado.categoria;
          this.lblmarca = dtl.listado.marca;
          this.lblmarcaoriginal = dtl.listado.marcaoriginal;
          this.lblmedida = dtl.listado.medida;
          this.lblmodelo = dtl.listado.modelo;
          this.lblmedidaestandarizado = dtl.listado.medidaestandarizado;
          this.lblid = dtl.listado.id;
          this.lblcodigo = dtl.listado.codigo;
          this.lblfamilia = dtl.listado.familia;
          this.lblsubfamilia = dtl.listado.subfamilia;
          this.lbltipo = dtl.listado.tipo;

          this.lblcodigoequivalente = dtl.listado.codigoequivalente;
          this.lblaplicacion = dtl.listado.aplicacion;
          this.lblproducto = dtl.listado.producto;
          this.lbltipo1 = dtl.listado.tipo1;

          this.lblpathoriginal = dtl.primeraRutaOriginal;
          this.Listrutaoriginal = dtl.rutasOriginales;
          this.lbltotalImagenes = dtl.totalImagenes;
        },
      });
    });
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

  AgregarCarrito() {
    const product = {
      descripcion: this.lbldescripcion,
      unidad: this.lblunidadmedida,
      categoria: this.lblcategoria,
      marca: this.lblmarca,
      marcaoriginal: this.lblmarcaoriginal,
      medida: this.lblmedida,
      modelo: this.lblmodelo,
      medidaestandarizado: this.lblmedidaestandarizado,
      id: this.lblid,
      codigo: this.lblcodigo,
      familia: this.lblfamilia,
      subfamilia: this.lblsubfamilia,
      tipo: this.lbltipo,
      cantidad: this.count ?? 1,

      sku: this.lblcodigo,
      producto: this.lblmarcaoriginal,
      vendor: this.lblmarca,
      quantity: this.count,
      color: '',
    };
    this.cotizacionService.addToCart2(product);
    this.cartSubscription = this.cotizacionService.cart$.subscribe((items) => {
      this.ListCarrito = items;
    });
  }

  DtlImagen1(item: string) {
    this.lblpathoriginal = item; // Cambia la imagen principal a la que se hizo clic
  }
  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
