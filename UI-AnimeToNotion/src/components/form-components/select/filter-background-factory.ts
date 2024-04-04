import { MAT_SELECT_CONFIG } from '@angular/material/select';
import { Injectable } from '@angular/core';

@Injectable()
export class SelectConfigFactory {

  constructor() {  }

  getConfig(isFilter: boolean) {
    if (isFilter) {
      return { overlayPanelClass: ['mat-override-cdk', 'filter-panel'] };
    } else {
      return { overlayPanelClass: ['mat-override-cdk'] };
    }
  }
}
