import { Component, HostListener, OnInit } from '@angular/core';
import { SharedMain } from '../../../sharedmain';
import { PreloaderComponent } from '../../../helper/preloader/preloader.component';
import { MaestroclasificadoService } from '../../../../core/service/maestroclasificado.service';
import { AuthService } from '../../../../core/service/auth.service';

@Component({
  selector: 'app-listadollantas',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './listadollantas.component.html',
  styleUrls: ['./listadollantas.component.scss'],
})
export class ListadollantasComponent implements OnInit {
  public ListMedida: any[] = [];
  public ListModelo: any[] = [];
  public ListMarca: any[] = [];
  public ListCategoria: any[] = [];

  public NgMolmedida: string = '';
  public NgMolmodelo: string = '';
  public NgMolmarca: string = '';
  public NgMolcategoria: string = '';

  public loading: boolean = false;
  public success: boolean = false;
  public isVisible: boolean = false;

  constructor(
    private servicesmaestro: MaestroclasificadoService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.auth.getRefreshToken();
    this.NgMolmedida = '';
    this.NgMolmodelo = '';
    this.NgMolmarca = '';
    this.NgMolcategoria = '';
    this.IniciadorDate();
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Verifica si el scroll es mayor a 200px
    this.isVisible = window.scrollY > 200;
  }

  subir() {
    // Desplaza la página hacia arriba
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
  IniciadorDate() {
    this.ListarMedidaLLanta();
    this.ListarModeloLLanta();
    this.ListarMarcaLLanta();
    this.ListarCategoriaLLanta();
  }

  ListarMedidaLLanta() {
    this.servicesmaestro.getListadoLLantaMedida().subscribe({
      next: (dtl: any[]) => {
        this.ListMedida = dtl;
      },
    });
  }

  ListarModeloLLanta() {
    this.servicesmaestro.getListadoLLantaModelo().subscribe({
      next: (dtl: any[]) => {
        this.ListModelo = dtl;
      },
    });
  }
  ListarMarcaLLanta() {
    this.servicesmaestro.getListadoLLantaMarca().subscribe({
      next: (dtl: any[]) => {
        this.ListMarca = dtl;
      },
    });
  }
  ListarCategoriaLLanta() {
    this.servicesmaestro.getListadoLLantaCategoria().subscribe({
      next: (dtl: any[]) => {
        this.ListCategoria = dtl;
      },
    });
  }
  SelectMedida() {}

  SelectModelo() {}
  SelectMarca() {}
  SelectCategoria() {}

  Limpiar() {
    this.loading = false;
    this.success = false;
    this.ListMedida = [];
    this.ListModelo = [];
    this.ListMarca = [];
    this.ListCategoria = [];
    this.NgMolmedida = '';
    this.NgMolmodelo = '';
    this.NgMolmarca = '';
    this.NgMolcategoria = '';
  }
}
