import { Component, OnInit } from '@angular/core';
import { SharedMain } from '../../../sharedmain';
import { PreloaderComponent } from "../../../helper/preloader/preloader.component";

@Component({
  selector: 'app-detallellantas',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './detallellantas.component.html',
  styleUrls: ['./detallellantas.component.scss'],
})
export class DetallellantasComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
