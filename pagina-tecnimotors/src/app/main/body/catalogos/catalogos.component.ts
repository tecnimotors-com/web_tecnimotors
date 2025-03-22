import {
  Component,
  HostListener,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { CatalogoService } from '../../../core/service/catalogo.service';
import { SharedMain } from '../../sharedmain';
import { PreloaderComponent } from '../../helper/preloader/preloader.component';
import { environment } from '../../../../environments/environment.development';
import { CotizacionService } from '../../../core/service/cotizacion.service';

@Component({
  selector: 'app-catalogos',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './catalogos.component.html',
  styleUrls: ['./catalogos.component.scss'],
})
export class CatalogosComponent implements OnInit {
  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Verifica si el scroll es mayor a 200px
    this.isVisible = window.scrollY > 200;
  }

  public isVisible: boolean = false;
  public txtlink: string =
    environment.myapiurlcatalogo + '/TipoCatalogo/GetBanner2?ruta=';

  public txttipocata: number = 0;
  public ListTipoCata: any[] = [];

  public tabpanel: string = '1';

  public array: any[] = [
    {
      text:
        this.txtlink +
        '/app/Imagen/Portadas_Tipo_Catalogo/Llantas_Camara_Moto_Bicicleta.jpg',
      text2: 'assets/img/banner/Catalogo/Llantas_Camara_Moto_Bicicleta.jpg',
      titulo: 'CATÁLOGOS DE LLANTAS Y CÁMARAS DE MOTOS Y BICICLETAS',
    },
    {
      text: this.txtlink + '/app/Imagen/Portadas_Tipo_Catalogo/Vehiculos.jpg',
      text2: 'assets/img/banner/Catalogo/Vehiculos.jpg',
      titulo: 'CATÁLOGOS DE VEHÍCULOS',
    },

    {
      text:
        this.txtlink +
        '/app/Imagen/Portadas_Tipo_Catalogo/Repuestos_Motos_Bicicletas.jpg',
      text2: 'assets/img/banner/Catalogo/Repuestos_Motos_Bicicletas.jpg',
      titulo: 'CATÁLOGOS DE REPUESTOS DE MOTOS Y BICICLETAS',
    },
    {
      text: this.txtlink + '/app/Imagen/Portadas_Tipo_Catalogo/Lubricantes.jpg',
      text2: 'assets/img/banner/Catalogo/Lubricantes.jpg',
      titulo: 'CATÁLOGOS DE LUBRICANTES',
    },
  ];

  constructor(
    private auth: AuthService,
    private catalogoservice: CatalogoService,
    private cotizacionservice: CotizacionService
  ) {}

  ngOnInit(): void {
    this.ListadoTipoCatalogo();
  }

  DetalleCatalogo(item: any) {
    console.log(item);
  }

  ListadoTipoCatalogo() {
    this.catalogoservice.getListarTipoCatalogoAll().subscribe({
      next: (lst: any) => {
        this.ListTipoCata = lst;
      },
    });
  }

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ChangeCatalogo() {
    this.catalogoservice.getDetailTipoCatalogoALl(this.txttipocata).subscribe({
      next: (lst: any) => {
        this.ListTipoCata = lst;
      },
    });
  }
}
