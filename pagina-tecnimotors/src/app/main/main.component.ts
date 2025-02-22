import { Component } from '@angular/core';
import { SharedMain } from './sharedmain';

@Component({
  selector: 'app-main',
  imports: [SharedMain],
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent {}
