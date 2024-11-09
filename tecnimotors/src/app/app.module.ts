import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
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
import { FilterbodyModule } from './filterbody/filterbody.module';

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
    FilterbodyModule,
    FooterModule,
    CommonModule,
    ReactiveFormsModule,
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent],
})
export class AppModule {}
