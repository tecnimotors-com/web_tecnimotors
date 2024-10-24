import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  public correo: string = 'ventas@tecnimotors.com';
  public isSticky: boolean = false;

  ngOnInit() {}
  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isSticky = window.scrollY > 0; // Cambia esto según tu necesidad
  }
}
