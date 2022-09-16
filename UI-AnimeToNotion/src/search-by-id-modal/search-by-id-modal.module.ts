import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MdbTooltipModule } from 'mdb-angular-ui-kit/tooltip';
import { SearchByIdModalComponent } from './search-by-id-modal.component';
import { SearchByIdModalService } from './search-by-id-modal.service';
import { HttpClientModule } from '@angular/common/http';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';



@NgModule({
  declarations: [
    SearchByIdModalComponent
  ],
  imports: [
    MdbTooltipModule,
    CommonModule,
    HttpClientModule,
    NgxSkeletonLoaderModule
  ],
  providers: [SearchByIdModalService],
  bootstrap: []
})
export class SearchByIdModalModule { }
