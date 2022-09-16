import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MdbTooltipModule } from 'mdb-angular-ui-kit/tooltip';
import { SearchByNameModalComponent } from './search-by-name-modal.component';
import { SearchByNameModalService } from './search-by-name-modal.service';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';


@NgModule({
  declarations: [
    SearchByNameModalComponent
  ],
  imports: [
    MdbTooltipModule,
    CommonModule,
    NgxSkeletonLoaderModule
  ],
  providers: [SearchByNameModalService],
  bootstrap: []
})
export class SearchByNameModule { }
