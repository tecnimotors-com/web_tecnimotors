import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import { RouterModule } from '@angular/router';
import { AngularSvgIconModule } from 'angular-svg-icon';

@NgModule({
  declarations: [HeaderComponent],
  imports: [CommonModule, RouterModule, AngularSvgIconModule],
  exports: [HeaderComponent], // Exporta el componente
})
export class HeaderModule {}
