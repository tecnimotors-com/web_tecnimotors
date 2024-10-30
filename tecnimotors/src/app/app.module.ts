import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { SvgIconRegistryService } from 'angular-svg-icon';
import { LazyLoadImageModule } from 'ng-lazyload-image';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProtectedComponent } from './body/protected/protected.component';
import { LoginComponent } from './auth/login/login.component';
import { HomeComponent } from './home/home.component';
/*-------------------------------*/
import { HeaderModule } from './header/header.module';
import { FooterModule } from './footer/footer.module';
import { HomebodyModule } from './homebody/homebody.module';

@NgModule({
  declarations: [
    AppComponent,
    ProtectedComponent,
    LoginComponent,
    HomeComponent,
  ],
  imports: [
    LazyLoadImageModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HeaderModule,
    HomebodyModule,
    FooterModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
