import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NosotrosComponent } from './nosotros/nosotros.component';
import { CategoriaComponent } from './categoria/categoria.component';
import { CotizacionComponent } from './cotizacion/cotizacion.component';
import { RegistromayoristaComponent } from './registromayorista/registromayorista.component';
import { CatalogosComponent } from './catalogos/catalogos.component';
import { BlogComponent } from './blog/blog.component';
import { ContactowspComponent } from './contactowsp/contactowsp.component';
import { HomebodyComponent } from './homebody.component';
import { HeaderModule } from '../header/header.module';
import { FooterModule } from '../footer/footer.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';

const routes: Routes = [
  {
    path: '',
    component: HomebodyComponent,
    children: [
      {
        path: 'nosotros',
        component: NosotrosComponent,
      },
      {
        path: 'categoria',
        component: CategoriaComponent,
      },
      {
        path: 'cotizacion',
        component: CotizacionComponent,
      },
      {
        path: 'mayorista',
        component: RegistromayoristaComponent,
      },
      {
        path: 'catalogo',
        component: CatalogosComponent,
      },
      {
        path: 'blog',
        component: BlogComponent,
      },
      {
        path: 'contactowsp',
        component: ContactowspComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [
    HomebodyComponent,
    NosotrosComponent,
    CategoriaComponent,
    CotizacionComponent,
    RegistromayoristaComponent,
    CatalogosComponent,
    BlogComponent,
    ContactowspComponent,
  ],
  imports: [
    NgxPaginationModule,
    NgSelectModule,
    CommonModule,
    FormsModule,
    RouterModule.forRoot(routes, { useHash: true }),
    HeaderModule,
    FooterModule,
    ReactiveFormsModule,
  ],
  exports: [
    HomebodyComponent,
    NosotrosComponent,
    CategoriaComponent,
    CotizacionComponent,
    RegistromayoristaComponent,
    CatalogosComponent,
    BlogComponent,
    ContactowspComponent,
  ],
})
export class HomebodyModule {}
