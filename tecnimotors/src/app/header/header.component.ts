import { Component, HostListener, OnInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { CotizacionService } from '../core/service/cotizacion.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  animations: [
    trigger('toggleSearch', [
      state(
        'hidden',
        style({
          opacity: 0,
          height: '0px',
          overflow: 'hidden',
        })
      ),
      state(
        'visible',
        style({
          opacity: 1,
          height: '*',
        })
      ),
      transition('hidden => visible', [animate('300ms ease-in')]),
      transition('visible => hidden', [animate('300ms ease-out')]),
    ]),
  ],
  standalone: false,
})
export class HeaderComponent implements OnInit {
  public isOpenCarShop: boolean = false;

  public isOpen: boolean = false;
  public correo: string = 'ventas@tecnimotors.com';
  public isSticky: boolean = false;
  public isSubMenuOpen: boolean = false;
  public isSubMenuOpen2: boolean = false;
  public divstyle: string =
    "display: flex;font-family: 'Rubik';font-size: 11px;font-weight: 600;justify-content: space-between;padding: 8px 10px;color:black";
  public titlestyle: string =
    /*"background-color: rgb(181, 181, 181);font-family: 'Rubik';font-size: 11px;font-weight: 600;padding: 8px 12px;";*/
    'background-color: #474747;font-family: unset;font-size: 11px;font-weight: 600;padding: 8px 12px;color: white;text-decoration: underline;';

  //public carritoAbierto: boolean = false;

  public motoscycle: any[] = [
    {
      title: 'VEHICULOS',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/homevehiculo/MOTOCI',
      /*
      subtitle: [
        {
          subtitle: 'MOTOCICLETAS',
          isOpen: false,
          style: this.divstyle,
          subsubtitle2: [
            {
              subsubtitle: 'MOTOS URBANAS',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'MOTOS SCOOTER',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'MOTOS DE TRABAJO',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'MOTOS TODO TERRENO',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'MOTOS CRUCEROS',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'MOTOS PISTERAS',
              isOpen: false,
              style: this.divstyle,
            },
          ],
        },
        {
          subtitle: 'BICICLETAS',
          isOpen: false,
          style: this.divstyle,
          subsubtitle2: [
            { subsubtitle: 'MONTAÑERA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'URBANA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'PLEGABLE', isOpen: false, style: this.divstyle },
            { subsubtitle: 'FREE STYLE', isOpen: false, style: this.divstyle },
            { subsubtitle: 'INFANTILES', isOpen: false, style: this.divstyle },
            { subsubtitle: 'TRICICLO', isOpen: false, style: this.divstyle },
          ],
        },
        { subtitle: 'CUATRIMOTOS', isOpen: false, style: this.divstyle },
        { subtitle: 'CARGUEROS', isOpen: false, style: this.divstyle },
      ],
      */
    },
    {
      title: 'LLANTAS Y CÁMARAS',
      isOpen: false,
      style: this.divstyle,
      subtitle: [
        {
          subtitle: 'LLANTAS',
          isOpen: false,
          style: this.divstyle,
          routerlink: '/homellantas',
          /*
          subsubtitle2: [
            {
              subsubtitle: 'DUNLOP',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/homellantas',
            },
            {
              subsubtitle: 'KENDA',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/filterllantas/DUNLOP',
            },
            {
              subsubtitle: 'CHENG SHIN',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/filterllantas/DUNLOP',
            },
            {
              subsubtitle: 'TSK',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/filterllantas/DUNLOP',
            },
            {
              subsubtitle: 'KATANA',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/filterllantas/DUNLOP',
            },
            {
              subsubtitle: 'WANDA',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/filterllantas/DUNLOP',
            },
            { subsubtitle: 'MAXXIS', isOpen: false, style: this.divstyle },
            { subsubtitle: 'SUNF', isOpen: false, style: this.divstyle },
          ],
          */
        },
        {
          subtitle: 'CÁMARAS',
          isOpen: false,
          style: this.divstyle,
          routerlink: '/homecamara/0/0',
          /*
          subsubtitle2: [
            {
              subsubtitle: 'DUNLOP',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/homecamara/0/DUNLOP',
            },
            {
              subsubtitle: 'KENDA',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/homecamara/0/KENDA',
            },
            {
              subsubtitle: 'CELIMO',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/homecamara/0/CELIMO',
            },
            {
              subsubtitle: 'CHENG SHIN',
              isOpen: false,
              style: this.divstyle,
              routerlink: '/homecamara/0/CS',
            },
         
          ],
          */
        },
      ],
    },
    {
      title: 'RESPUESTOS',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/homerepuesto/MOTOCICLETA',
      /*
      subtitle: [
        {
          subtitle: 'REPUESTOS PARA MOTOCICLETA',
          isOpen: false,
          style: this.divstyle,
          subsubtitle2: [
            {
              subsubtitle: 'CADENA DE ARRASTRE',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'CADENA DE ARRASTRE A',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE ARRASTRE ACC',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE ARRASTRE DID',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE ARRASTRE KMC',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE ARRASTRE MIB',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE ARRASTRE TEC',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'CADENA DE LEVA',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'CADENA DE LEVAS DID',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE LEVAS KMC',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENA DE LEVAS MIB',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'CATALINA MOTOCICLETA',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'CATALINA MIB',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CATALINA TECNI',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CATALINA TSK',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'BATERIA',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'BATERIA MIB',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'FILTROS',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'FILTRO DE ACEITE',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'FILTRO DE AIRE',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'FILTRO DE GASOLINA',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'FILTRO DE TRANSMISION',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'MOTORES MOTOCICLETAS',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'MOTORES RTM',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'ARRANCADOR',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CILINDRO',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'SISTEMA DE EJES Y RUEDAS MOTOCICLETAS',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'PIÑONES MOTOCICLETAS',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'SISTEMA DE CARBURACIÓN Y COMBUSTIBLE',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'CABEZAL',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CARBURADOR',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'SISTEMA DE FRENOS MOTOCICLETAS',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'PASTILLA DE FRENO',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'VARILLA DE FRENO',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'ZAPATA DE FRENO',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            {
              subsubtitle: 'SISTEMA DE SUSPENSIÓN',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'SISTEMA DE EMBRAGUE',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'SISTEMA ELÉCTRICO MOTOCICLETAS',
              isOpen: false,
              style: this.divstyle,
            },
            {
              subsubtitle: 'SISTEMA DE TRANSMISIÓN',
              isOpen: false,
              style: this.divstyle,
            },
          ],
        },
        {
          subtitle: 'REPUESTOS PARA BICICLETAS',
          isOpen: false,
          style: this.divstyle,
          subsubtitle2: [
            {
              subsubtitle: 'CADENAS KMC/TEC',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'CADENAS KMC',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'CADENAS TEC',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            { subsubtitle: 'AROS', isOpen: false, style: this.divstyle },
            {
              subsubtitle: 'ASIENTOS',
              isOpen: false,
              style: this.divstyle,
              thirdtitle: [
                {
                  submenutitle: 'GENÉRICO',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'HIGH HOPE',
                  isOpen: false,
                  style: this.divstyle,
                },
                {
                  submenutitle: 'MAGNUM',
                  isOpen: false,
                  style: this.divstyle,
                },
              ],
            },
            { subsubtitle: 'PEDALES', isOpen: false, style: this.divstyle },
            { subsubtitle: 'CATALINAS', isOpen: false, style: this.divstyle },
            { subsubtitle: 'FRENOS', isOpen: false, style: this.divstyle },
            { subsubtitle: 'PIÑONES', isOpen: false, style: this.divstyle },
            { subsubtitle: 'RAYOS', isOpen: false, style: this.divstyle },
          ],
        },
        {
          subtitle: 'REPUESTOS PARA TRIMOTO DE CARGA',
          isOpen: false,
          style: this.divstyle,
          subsubtitle2: [
            { subsubtitle: 'CABEZAL', isOpen: false, style: this.divstyle },
            { subsubtitle: 'CILINDROS', isOpen: false, style: this.divstyle },
          ],
        },
      ],
      */
    },
    {
      title: 'ACEITES Y LUBRICANTES',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/homeaceite/98',
    },
    /*
    { title: 'ACCESORIOS', isOpen: false, style: this.divstyle, subtitle: [] },
     */
  ];

  public blog: any[] = [
    {
      title: 'NOSOTROS',
      isOpen: false,
      style: this.divstyle,
      routerlink: '/nosotros',
    },
  ];

  public hdnSearch: boolean = true;
  public ListCarrito: any[] = [];

  constructor(private cotizacionService: CotizacionService) {}
  ngOnInit(): void {}

  toggleblogmenu(menuItem: any) {
    const isCurrentlyOpen = menuItem.isOpen;
    this.blog.forEach((item: any) => {
      item.isOpen = false;
      item.isSelected = false;
    });
    if (!isCurrentlyOpen) {
      menuItem.isOpen = true;
      menuItem.isSelected = true;
    }
  }

  toggleMenuprueba(menuItem: any) {
    const isCurrentlyOpen = menuItem.isOpen;
    this.motoscycle.forEach((item: any) => {
      item.isOpen = false;
      item.isSelected = false;
    });
    if (!isCurrentlyOpen) {
      menuItem.isOpen = true;
      menuItem.isSelected = true;
    }
  }

  toggleSubMenuprueba(subItem: any) {
    subItem.isOpen = !subItem.isOpen;

    this.motoscycle.forEach((menu) => {
      // Verifica si menu.subtitle está definido y es un array
      if (Array.isArray(menu.subtitle)) {
        menu.subtitle.forEach((item: any) => {
          if (item !== subItem) {
            item.isOpen = false; // Cerrar otros subelementos
            item.isSelected = false; // Desmarcar otros subelementos
          } else {
            item.isSelected = !item.isSelected; // Alternar selección del subItem
          }
        });
      }
    });
  }

  /*toggleSubMenuprueba(subItem: any) {
    subItem.isOpen = !subItem.isOpen;
    this.motoscycle.forEach((menu) => {
      menu.subtitle.forEach((item: any) => {
        if (item !== subItem) {
          item.isOpen = false;
          item.isSelected = false;
        }
      });
    });
  }*/
  /*
  toggleSubMenuprueba(subItem: any) {
    subItem.isOpen = !subItem.isOpen;
    this.motoscycle.forEach((menu) => {
      menu.subtitle.forEach((item: any) => {
        if (item !== subItem) {
          item.isOpen = false; // Cerrar otros subelementos
          item.isSelected = false; // Desmarcar otros subelementos
        } else {
          item.isSelected = !item.isSelected; // Alternar selección del subItem
        }
      });
    });
  }
*/
  toggleSubsubMenuprueba(subSubItem: any) {
    const parentMenu = this.motoscycle.find((item: any) =>
      item.subtitle.some(
        (sub: any) =>
          Array.isArray(sub.subsubtitle2) &&
          sub.subsubtitle2.includes(subSubItem)
      )
    );

    if (parentMenu) {
      parentMenu.subtitle.forEach((sub: any) => {
        if (Array.isArray(sub.subsubtitle2)) {
          sub.subsubtitle2.forEach((item: any) => {
            if (item !== subSubItem) {
              item.isOpen = false;
              item.isSelected = false;
            }
          });
        }
      });
    }
    subSubItem.isOpen = !subSubItem.isOpen;
    subSubItem.isSelected = !subSubItem.isSelected;
  }

  togglesubmenusegundo(thirdtitle: any) {
    this.motoscycle.forEach((menu) => {
      menu.subtitle.forEach((subItem: any) => {
        if (subItem.thirdtitle) {
          subItem.thirdtitle.forEach((item: any) => {
            item.isSelected = false;
          });
        }
      });
    });
    thirdtitle.isSelected = true;
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isSticky = window.scrollY > 0; // Cambia esto según tu necesidad
  }

  abrirCarrito(event: MouseEvent): void {
    event.stopPropagation(); // Evita que el evento de clic se propague
    this.isOpenCarShop = !this.isOpenCarShop; // Alterna el estado del carrito
    this.isOpen = false; // Cierra el otro offcanvas si está abierto
    this.ListCarrito = this.cotizacionService.getCartItems();
    console.log(this.ListCarrito);
  }

  cerrarCarrito(): void {
    this.isOpenCarShop = false; // Cierra el carrito
  }

  toggleOffcanvas(event: MouseEvent): void {
    event.stopPropagation(); // Evita que el evento de clic se propague
    this.isOpen = !this.isOpen; // Alterna el estado del otro offcanvas
    this.isOpenCarShop = false; // Cierra el carrito si está abierto
  }
  // Método para prevenir el cierre al hacer clic dentro del offcanvas
  preventClose(event: MouseEvent): void {
    event.stopPropagation(); // Previene que el clic se propague al documento
  }

  closeOffcanvas(): void {
    this.isOpen = false; // Cierra el otro offcanvas
  }

  clicksearch(): void {
    this.hdnSearch = !this.hdnSearch;
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;

    // Verifica si el clic se realizó dentro del offcanvas o del botón de abrir
    const isClickInsideOffcanvas = target.closest('.offcanvas__header');
    const isClickInsideToggleButton = target.closest(
      '.offcanvas__header--menu__open--btn'
    );
    // Cierra el offcanvas si se hace clic fuera de él
    if (this.isOpen && !isClickInsideOffcanvas && !isClickInsideToggleButton) {
      this.closeOffcanvas();
    }

    // Verifica si el clic se realizó dentro del carrito
    const isClickInsideMinicart = target.closest('.offcanvas__minicart');
    const isclickInsideToggleButton = target.closest(
      '.offcanvas__header--menu__open--btn2'
    );
    if (this.isOpenCarShop && !isclickInsideToggleButton) {
      this.cerrarCarrito();
    }

    // Cierra el submenú si se hace clic fuera de él
    if (
      !target.closest('.header__sub--menu') &&
      !target.closest('.header__menu--items')
    ) {
      this.isSubMenuOpen = false;
    }
  }

  get searchState() {
    return this.hdnSearch ? 'hidden' : 'visible';
  }

  // Método para aumentar la cantidad
  aumentarCantidad(item: any): void {
    const existingProductIndex = this.ListCarrito.findIndex(
      (product) => product.codigo === item.codigo
    );
    if (existingProductIndex !== -1) {
      this.ListCarrito[existingProductIndex].cantidad += 1; // Aumenta la cantidad
    }
  }

  // Método para disminuir la cantidad
  disminuirCantidad(item: any): void {
    const existingProductIndex = this.ListCarrito.findIndex(
      (product) => product.codigo === item.codigo
    );
    if (existingProductIndex !== -1) {
      if (this.ListCarrito[existingProductIndex].cantidad > 1) {
        this.ListCarrito[existingProductIndex].cantidad -= 1; // Disminuye la cantidad
      }
    }
  }

  // Método para eliminar un producto del carrito
  eliminarProducto(item: any): void {
    this.ListCarrito = this.ListCarrito.filter(
      (product) => product.codigo !== item.codigo
    );
  }
}
