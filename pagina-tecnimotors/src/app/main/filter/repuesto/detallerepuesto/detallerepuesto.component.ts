import { Component, OnInit } from '@angular/core';
import { SharedMain } from '../../../sharedmain';
import { PreloaderComponent } from '../../../helper/preloader/preloader.component';

@Component({
  selector: 'app-detallerepuesto',
  imports:[SharedMain, PreloaderComponent],
  templateUrl: './detallerepuesto.component.html',
  styleUrls: ['./detallerepuesto.component.scss']
})
export class DetallerepuestoComponent implements OnInit {
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

}
