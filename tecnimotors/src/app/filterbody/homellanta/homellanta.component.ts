import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { MaestroarticuloService } from '../../core/service/maestroarticulo.service';
import { Swiper } from 'swiper';

@Component({
  selector: 'app-homellanta',
  templateUrl: './homellanta.component.html',
  styleUrls: ['./homellanta.component.css'],
})
export class HomellantaComponent implements OnInit, OnDestroy, AfterViewInit {
  public titlellanta: string = 'General';
  public listcocada: any[] = [];
  public listmarca: any[] = [];
  public listtipouso: any[] = [];
  public listarticulo: any[] = [];
  public listinicio: any[] = [];

  public txtModelo: string = '';
  public selectedCocada: string = '';
  public txtanchoperfil: string = '';
  public txttipouso: string = '';

  public loading = false;
  public success = false;
  public loading2 = false;
  public success2 = false;
  public loading3 = false;
  public success3 = false;

  constructor(private servicesmaestro: MaestroarticuloService) {}
  ngAfterViewInit() {
    const swiper = new Swiper('.swiper-container', {
      // opciones de configuración
    });
  }
  ngOnInit(): void {
    this.ListadoArticulo();
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ListadoArticulo() {
    this.servicesmaestro.getMaestroArticulo().subscribe({
      next: (dtl: any[]) => {
        this.listinicio = dtl
          .map((item) => {
            // Acceder al índice 3 para obtener 'cocada'
            const cocada = item.ancho;
            return cocada; // Eliminar comillas y espacios
          })
          .filter((cocada) => cocada);
      },
    });
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

  select(event: Event): void {
    const selectElement = event.target as HTMLSelectElement; // Obtiene el elemento select
    const selectedValue =
      selectElement.options[selectElement.selectedIndex].text; // Obtiene el texto de la opción seleccionada

    // Verifica si se seleccionó "Marca"
    this.titlellanta = selectedValue === 'Marca' ? 'General' : selectedValue;
  }

  SelectMarca() {
    if (this.txtModelo == '') {
      this.listtipouso = [];
    } else {
      this.loading3 = true;
      this.success3 = false;
      var buscar = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
      var cocada = this.selectedCocada == null ? '' : this.selectedCocada;
      const frombody: any = {
        value: buscar,
        cocada: cocada,
      };
      if (frombody.cocada == '') {
        setTimeout(() => {
          this.loading3 = false;
          this.success3 = true;
          this.txtModelo = '';
          this.listtipouso = [];
          setTimeout(() => {
            this.success3 = false;
          }, 1000);
        }, 1000);
      } else {
        this.servicesmaestro.getAllFiltroMarcaCocada(frombody).subscribe({
          next: (dtl: any[]) => {
            setTimeout(() => {
              //this.selectedCocada = ;
              this.loading3 = false;
              this.success3 = true;
              this.listtipouso = [];
              this.listtipouso = dtl
                .map((item) => {
                  const tipouso = item.tipouso;
                  return tipouso ? tipouso : 'no tiene';
                })
                .filter(
                  (tipouso, index, self) => self.indexOf(tipouso) === index
                );
              setTimeout(() => {
                this.success3 = false;
              }, 1000);
            }, 1000);
          },
          error: () => {
            this.loading3 = false;
          },
        });
      }
    }
  }

  SelectCocada() {
    if (this.selectedCocada == '') {
      this.listmarca = [];
    } else {
      this.loading2 = true;
      this.success2 = false;
      var buscar = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
      var cocada = this.selectedCocada == null ? '' : this.selectedCocada;
      const frombody: any = {
        value: buscar,
        cocada: cocada,
      };
      if (frombody.cocada == '') {
        setTimeout(() => {
          this.loading2 = false;
          this.success2 = true;
          this.txtModelo = '';
          this.listmarca = [];
          setTimeout(() => {
            this.success2 = false;
          }, 1000);
        }, 1000);
      } else {
        this.servicesmaestro.getAllFiltroMarcaCocada(frombody).subscribe({
          next: (dtl: any[]) => {
            setTimeout(() => {
              //this.selectedCocada = ;
              this.loading2 = false;
              this.success2 = true;
              this.listmarca = [];
              this.listarticulo = dtl;
              this.listmarca = dtl
                .map((item) => item.marcaoriginal)
                .filter(
                  (marca, index, self) => marca && self.indexOf(marca) === index
                );
              console.log(this.listarticulo);
              setTimeout(() => {
                this.success2 = false;
              }, 1000);
            }, 1000);
          },
          error: () => {
            this.loading2 = false;
          },
        });
      }
    }
  }

  inputfiel() {
    this.loading = true;
    this.success = false;
    var buscar = this.txtanchoperfil == null ? '' : this.txtanchoperfil;
    const frombody: any = {
      value: buscar,
    };
    if (frombody.value == '') {
      this.loading2 = true;
      this.success2 = false;
      this.loading3 = true;
      this.success3 = false;
      setTimeout(() => {
        this.loading = false;
        this.success = true;
        this.loading2 = false;
        this.success2 = true;
        this.loading3 = false;
        this.success3 = true;
        this.selectedCocada = '';
        this.listcocada = [];
        this.txtModelo = '';
        this.listmarca = [];
        this.txttipouso = '';
        this.listtipouso = [];
        setTimeout(() => {
          this.success = false;
          this.success2 = false;
          this.success3 = false;
        }, 1000);
      }, 1000);
    } else {
      this.servicesmaestro.getAllSinFiltroArticulo(frombody).subscribe({
        next: (dtl: any[]) => {
          setTimeout(() => {
            //this.selectedCocada = ;
            this.loading = false;
            this.success = true;
            this.listcocada = [];
            this.listcocada = dtl
              .map((item) => item.cocada)
              .filter(
                (cocada, index, self) =>
                  cocada && self.indexOf(cocada) === index
              );

            setTimeout(() => {
              this.success = false;
            }, 1000);
          }, 1000);
        },
        error: () => {
          this.loading = false;
        },
      });
    }
  }
}
