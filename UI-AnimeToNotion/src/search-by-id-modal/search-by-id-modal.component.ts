import { Component, OnInit } from '@angular/core';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { MAL_AnimeModel } from '../model/MAL_AnimeModel';
import { SearchByIdModalService } from './search-by-id-modal.service';

@Component({
  selector: 'app-search-by-id-modal',
  templateUrl: './search-by-id-modal.component.html',
  styleUrls: ['./search-by-id-modal.component.css']
})
export class SearchByIdModalComponent implements OnInit {

  animeModel: MAL_AnimeModel | null = null;
  id: string | null = null;
  errorMessage: string | null = null;

  multiSeasonEditBtn: boolean = false;
  multiSeasonCheckBtn: boolean = false;
  multiSeasonInputDisabled: boolean = false;

  tooltipDelay: number = 200;
  progressValue: number = 50;
  progressPercentage: string = "width: [value]%";

  loading: boolean = true;

  constructor(private service: SearchByIdModalService, public modalRef: MdbModalRef<SearchByIdModalComponent>) {

  }

  ngOnInit(): void {
    this.multiSeasonEditBtn = true;
    this.multiSeasonCheckBtn = false;
    this.multiSeasonInputDisabled = true;

    this.getShowById(this.id!);
  }

  getShowById(id: string) {
    this.service.getShowById(id)
      .subscribe(
        (data: MAL_AnimeModel) => {
          this.animeModel = data;
          this.progressValue = Math.trunc(this.animeModel?.mean! * 10);
          this.progressPercentage = this.progressPercentage.replace("[value]", this.progressValue.toString());

          setInterval(() => this.loading = false, 100);
        },
        error => {
          this.errorMessage = error;
          this.loading = false;
        }
    );    
  }


  //Multi Season Identifier Methods
  editMultiSeasonId() {
    this.multiSeasonEditBtn = false;
    this.multiSeasonCheckBtn = true;

    this.multiSeasonInputDisabled = false;
  }

  confirmMultiSeasonId() {
    this.multiSeasonEditBtn = true;
    this.multiSeasonCheckBtn = false;

    this.multiSeasonInputDisabled = true;
  }

  remapMediaType(media_type: string) {
    var result: string = "TV Show";

    switch (media_type) {
      case "tv":
      default:
        result = "TV Show";
        break;
      case "movie":
        result = "Movie";
        break;
      case "ova":
        result = "OVA";
        break;
      case "special":
        result = "Special";
        break;
    }

    return result;
  }
}
