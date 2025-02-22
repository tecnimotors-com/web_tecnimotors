import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
  NgbPaginationModule,
  NgbAlertModule,
  NgbModule,
  NgbPopoverModule,
} from '@ng-bootstrap/ng-bootstrap';

export const SharedHeader = [
  CommonModule,
  RouterModule,
  NgbAlertModule,
  NgbPaginationModule,
  FormsModule,
  NgbModule,
  NgbPopoverModule,
];
