import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { MAL_AnimeModel } from '../model/MAL_AnimeModel';
import { SearchByIdModalService } from './search-by-id-modal.service';
import { Notify } from 'notiflix/build/notiflix-notify-aio';

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
  multiSeasonIdentifier: string | null = null;

  tooltipDelay: number = 200;
  progressValue: number = 50;
  progressPercentage: string = "width: [value]%";

  loading: boolean = true;
  addingLoading: boolean = false;

  constructor(private service: SearchByIdModalService, public modalRef: MdbModalRef<SearchByIdModalComponent>) {

  }

  ngOnInit(): void {
    Notify.init({ position: 'center-top' });

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

  postAddToNotion(show: MAL_AnimeModel, event: any) {
    this.addingLoading = true;
    show.showHidden = this.multiSeasonIdentifier !== "" ? this.multiSeasonIdentifier : null;
    event.target.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';

    this.service.postAddToNotion(show)
      .subscribe(
        () => {
          Notify.success('"' + show.title + '" correctly sent to Notion');
          event.target.innerHTML = '<span>Add to Notion</span>';
          this.addingLoading = false;
        },
        error => {
          Notify.failure(error.error);
          event.target.innerHTML = '<span>Add to Notion</span>';
          this.addingLoading = false;
        }
      );
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
