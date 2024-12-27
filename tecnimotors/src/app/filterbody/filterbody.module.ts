import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { HeaderModule } from '../header/header.module';
import { FooterModule } from '../footer/footer.module';
import { LightgalleryModule } from 'lightgallery/angular';
import { NgxPaginationModule } from 'ngx-pagination';
/*-----------------------------------------------*/
import { FilterbodyComponent } from './filterbody.component';
import { HomellantaComponent } from './llantas/homellanta/homellanta.component';
import { DetallellantaComponent } from './llantas/detallellanta/detallellanta.component';
import { HomecamaraComponent } from './camaras/homecamara/homecamara.component';
import { DetallecamaraComponent } from './camaras/detallecamara/detallecamara.component';
import { HomerepuestoComponent } from './repuesto/homerepuesto/homerepuesto.component';
import { DetallerepuestoComponent } from './repuesto/detallerepuesto/detallerepuesto.component';
import { HomeaceiteComponent } from './aceite/homeaceite/homeaceite.component';
import { DetalleaceiteComponent } from './aceite/detalleaceite/detalleaceite.component';
import { HomevehiculoComponent } from './vehiculo/homevehiculo/homevehiculo.component';
import { DetallevehiculoComponent } from './vehiculo/detallevehiculo/detallevehiculo.component';

const routes: Routes = [
  {
    path: '',
    component: FilterbodyComponent,
    children: [
      /*---------llanta--------*/
      {
        path: 'homellantas',
        component: HomellantaComponent,
      },
      {
        path: 'detallellanta/:id',
        component: DetallellantaComponent,
      },
      /*--------camara---------*/
      {
        path: 'homecamara/:tipo/:marca',
        component: HomecamaraComponent,
      },
      {
        path: 'detallecamara/:id',
        component: DetallecamaraComponent,
      },
      /*--------repuesto---------*/
      {
        path: 'homerepuesto',
        component: HomerepuestoComponent,
      },
      {
        path: 'detallerepuesto/:id',
        component: DetallerepuestoComponent,
      },
      /*---------aceite--------*/
      {
        path: 'homeaceite',
        component: HomeaceiteComponent,
      },
      {
        path: 'detalleaceite/:id',
        component: DetalleaceiteComponent,
      },
      /*---------vehiculos----------*/
      {
        path: 'homevehiculo',
        component: HomevehiculoComponent,
      },
      {
        path: 'detallevehiculo',
        component: DetallevehiculoComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [
    FilterbodyComponent,
    HomellantaComponent,
    DetallellantaComponent,
    HomecamaraComponent,
    DetallecamaraComponent,
    HomerepuestoComponent,
    DetallerepuestoComponent,
    HomeaceiteComponent,
    DetalleaceiteComponent,
    HomevehiculoComponent,
    DetallevehiculoComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forRoot(routes, { useHash: true }),
    ReactiveFormsModule,
    NgSelectModule,
    HeaderModule,
    FooterModule,
    LightgalleryModule,
    NgxPaginationModule,
  ],
  exports: [
    FilterbodyComponent,
    HomellantaComponent,
    DetallellantaComponent,
    HomecamaraComponent,
    DetallecamaraComponent,
    HomerepuestoComponent,
    DetallerepuestoComponent,
    HomeaceiteComponent,
    DetalleaceiteComponent,
    HomevehiculoComponent,
    DetallevehiculoComponent,
  ],
})
export class FilterbodyModule {}
