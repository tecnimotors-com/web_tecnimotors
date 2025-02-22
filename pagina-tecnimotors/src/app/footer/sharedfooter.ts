import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {
  NgbPaginationModule,
  NgbAlertModule,
  NgbModule,
  NgbPopoverModule,
} from '@ng-bootstrap/ng-bootstrap';

export const SharedFooter = [
  CommonModule,
  RouterModule,
  NgbAlertModule,
  NgbPaginationModule,
  NgbModule,
  NgbPopoverModule,
];
