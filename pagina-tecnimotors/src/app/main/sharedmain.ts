import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';

export const SharedMain = [
  RouterModule,
  NgxPaginationModule,
  NgSelectModule,
  CommonModule,
  FormsModule,
  HeaderComponent,
  FooterComponent,
  ReactiveFormsModule,
];
