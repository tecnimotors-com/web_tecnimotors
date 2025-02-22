import { Component } from '@angular/core';
import { SharedFooter } from './sharedfooter';

@Component({
  selector: 'app-footer',
  imports: [SharedFooter],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
})
export class FooterComponent {
  public correofooter: string = 'ventas@tecnimotors.com';
  public footertitulo: string =
    'Representaciones Tecnimotors EIRL 2024 © Todos los Derechos Reservados';
}
