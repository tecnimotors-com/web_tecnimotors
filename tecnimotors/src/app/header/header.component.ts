import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  public correo: string = 'ventas@tecnimotors.com';
  public isSticky: boolean = false;
  public isSubMenuOpen: boolean = false;
  public isSubMenuOpen2: boolean = false;
  public divstyle: string =
    "display: flex;font-family: 'Rubik';font-size: 11px;font-weight: 600;justify-content: space-between;padding: 8px 10px;";
  public titlestyle: string =
    /*"background-color: rgb(181, 181, 181);font-family: 'Rubik';font-size: 11px;font-weight: 600;padding: 8px 12px;";*/
    'background-color: #474747;font-family: unset;font-size: 11px;font-weight: 600;padding: 8px 12px;color: white;text-decoration: underline;';

  public carritoAbierto: boolean = false;
  public motoscycle: any[] = [
    {
      title: 'VEHICULOS',
      isOpen: false,
      style: this.divstyle,
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
          subsubtitle2: [
            { subsubtitle: 'DUNLOP', isOpen: false, style: this.divstyle },
            { subsubtitle: 'KENDA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'CHENG SHIN', isOpen: false, style: this.divstyle },
            { subsubtitle: 'TSK', isOpen: false, style: this.divstyle },
            { subsubtitle: 'KATANA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'WANDA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'MAXXIS', isOpen: false, style: this.divstyle },
            { subsubtitle: 'SUNF', isOpen: false, style: this.divstyle },
          ],
        },
        {
          subtitle: 'CÁMARAS',
          isOpen: false,
          style: this.divstyle,
          subsubtitle2: [
            { subsubtitle: 'DUNLOP', isOpen: false, style: this.divstyle },
            { subsubtitle: 'KENDA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'CHENG SHIN', isOpen: false, style: this.divstyle },
            { subsubtitle: 'TSK', isOpen: false, style: this.divstyle },
            { subsubtitle: 'KATANA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'WANDA', isOpen: false, style: this.divstyle },
            { subsubtitle: 'CELIMO', isOpen: false, style: this.divstyle },
            { subsubtitle: 'MIB', isOpen: false, style: this.divstyle },
          ],
        },
      ],
    },
    {
      title: 'RESPUESTOS',
      isOpen: false,
      style: this.divstyle,
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
    },
    {
      title: 'ACEITES Y LUBRICANTES',
      isOpen: false,
      style: this.divstyle,
      subtitle: [],
    },
    { title: 'ACCESORIOS', isOpen: false, style: this.divstyle, subtitle: [] },
  ];

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
      menu.subtitle.forEach((item: any) => {
        if (item !== subItem) {
          item.isOpen = false;
        }
      });
    });
  }

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
    thirdtitle.isOpen = !thirdtitle.isOpen;
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isSticky = window.scrollY > 0; // Cambia esto según tu necesidad
  }

  toggleSubMenu(event: MouseEvent) {
    event.stopPropagation();
    this.isSubMenuOpen = !this.isSubMenuOpen;
  }
  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (
      !target.closest('.header__sub--menu') &&
      !target.closest('.header__menu--items')
    ) {
      this.isSubMenuOpen = false;
    }
  }

  toggleSubMenu2(event: MouseEvent) {
    event.stopPropagation();
    this.isSubMenuOpen2 = !this.isSubMenuOpen2;
  }

  abrirCarrito() {
    this.carritoAbierto = !this.carritoAbierto;
  }
  cerrarCarrito() {
    this.carritoAbierto = false;
  }

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const isClickInside =
      target.closest('.offCanvas__minicart') ||
      target.closest('.minicart__open--btn');

    if (!isClickInside) {
      this.cerrarCarrito();
    }
  }
}
