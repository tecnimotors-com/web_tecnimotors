import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
  standalone: false, // Asegúrate de que las rutas sean correctas
})
export class FooterComponent {
  public correofooter: string = 'ventas@tecnimotors.com';
  public footertitulo: string =
    'Representaciones Tecnimotors EIRL 2024 © Todos los Derechos Reservados';
}
