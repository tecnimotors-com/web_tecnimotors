import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-homerepuesto',
  templateUrl: './homerepuesto.component.html',
  styleUrls: ['./homerepuesto.component.css'],
})
export class HomerepuestoComponent implements OnInit {
  txttipoVehiculo: string | null = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.txttipoVehiculo = params['tipo'];
      console.log('Tipo de vehículo recibido:', this.txttipoVehiculo);
    });
  }
}
