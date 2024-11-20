import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
/*-----------------------------------------------*/
import { FilterbodyComponent } from './filterbody.component';
import { HomellantaComponent } from './homellanta/homellanta.component';
import { DetallellantaComponent } from './detallellanta/detallellanta.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { HeaderModule } from "../header/header.module";
import { FooterModule } from '../footer/footer.module';

const routes: Routes = [
  {
    path: '',
    component: FilterbodyComponent,
    children: [
      {
        path: 'homellantas',
        component: HomellantaComponent,
      },
      {
        path: 'detallellanta',
        component: DetallellantaComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [
    FilterbodyComponent,
    HomellantaComponent,
    DetallellantaComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forRoot(routes, { useHash: true }),
    ReactiveFormsModule,
    NgSelectModule,
    HeaderModule,
    FooterModule
],
  exports: [FilterbodyComponent, HomellantaComponent, DetallellantaComponent],
})
export class FilterbodyModule {}
