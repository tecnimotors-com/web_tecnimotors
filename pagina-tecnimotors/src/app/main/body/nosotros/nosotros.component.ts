import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../../core/service/auth.service';
import { SharedMain } from '../../sharedmain';
import { PreloaderComponent } from "../../helper/preloader/preloader.component";

@Component({
  selector: 'app-nosotros',
  imports: [SharedMain, PreloaderComponent],
  templateUrl: './nosotros.component.html',
  styleUrls: ['./nosotros.component.scss'],
})
export class NosotrosComponent implements OnInit, OnDestroy {
  constructor(private auth: AuthService) {}
  ngOnInit(): void {
    this.auth.getRefreshToken();
  }

  ngOnDestroy(): void {}
}
