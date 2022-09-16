import { Component } from '@angular/core';
import { MdbModalConfig, MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { MdbTooltipComponent } from 'mdb-angular-ui-kit/tooltip';
import { MAL_AnimeModel } from '../model/MAL_AnimeModel';
import { SearchByIdModalComponent } from '../search-by-id-modal/search-by-id-modal.component';
import { SearchByNameModalComponent } from '../search-by-name-modal/search-by-name-modal.component';
import { AppService } from './app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'UI-AnimeToNotion';

  searchByIdComp: MdbModalRef<SearchByIdModalComponent> | null = null;
  searchByNameComp: MdbModalRef<SearchByNameModalComponent> | null = null;
  modalConfig: MdbModalConfig | null = null;

  showFound: MAL_AnimeModel | null = null;
  errorMessage: string | null = null;

  constructor(private service: AppService, private modalService: MdbModalService) { }

  openByIdModal(value: string) {
    this.searchByIdComp = this.modalService.open(SearchByIdModalComponent, { data: { id: value }, modalClass: 'modal-dialog-centered modal-lg' });
  }

  openByNameModal(value: string) {
    this.searchByNameComp = this.modalService.open(SearchByNameModalComponent, { data: { searchTerm: value }, modalClass: 'modal-dialog-centered modal-xl' })
  }
}
