import { Component } from '@angular/core';
import { MdbModalConfig, MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { MdbTooltipComponent } from 'mdb-angular-ui-kit/tooltip';
import { ThemeService } from '../components/utility-components/theme/theme.service';
import { MAL_AnimeModel } from '../model/MAL_AnimeModel';
import { Notion_LatestAddedModel } from '../model/Notion_LatestAddedModel';
import { SearchByIdModalComponent } from '../search-by-id-modal/search-by-id-modal.component';
import { SearchByNameModalComponent } from '../search-by-name-modal/search-by-name-modal.component';
import { AppService } from './app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'UI-AnimeToNotion';

  latestAdded: Notion_LatestAddedModel[] | null = null;

  searchByIdComp: MdbModalRef<SearchByIdModalComponent> | null = null;
  searchByNameComp: MdbModalRef<SearchByNameModalComponent> | null = null;
  modalConfig: MdbModalConfig | null = null;

  showFound: MAL_AnimeModel | null = null;
  errorMessage: string | null = null;
  loading: boolean = true;
  

  constructor(private service: AppService, private modalService: MdbModalService, private themeService: ThemeService) { }

  ngOnInit(): void {
    this.getLatestAdded();
  }


  getLatestAdded() {
    this.service.getLatestAdded()
      .subscribe(
        (data) => {
          this.latestAdded = data;
          this.loading = false;
        }, error => {
          this.errorMessage = error;
          this.loading = false;
        }
      );
  }



  openByIdModal(value: string) {
    this.searchByIdComp = this.modalService.open(SearchByIdModalComponent, { data: { id: value }, modalClass: 'modal-dialog-centered modal-lg' });
  }

  openByNameModal(value: string) {
    this.searchByNameComp = this.modalService.open(SearchByNameModalComponent, { data: { searchTerm: value }, modalClass: 'modal-dialog-centered modal-xl' })
  }

  switchTheme(darkTheme: boolean) {
    this.themeService.switchTheme(darkTheme);
  }

  isDarkTheme() {
    return this.themeService.isCurrentlyDarkTheme();
  }
}
