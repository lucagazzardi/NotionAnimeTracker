import { Component, OnInit } from '@angular/core';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { animeListMocked, MAL_AnimeModel } from '../model/MAL_AnimeModel';
import { SearchByNameModalService } from './search-by-name-modal.service';
import { Notify } from 'notiflix/build/notiflix-notify-aio';

@Component({
  selector: 'app-search-by-name-modal',
  templateUrl: './search-by-name-modal.component.html',
  styleUrls: ['./search-by-name-modal.component.css']
})
export class SearchByNameModalComponent implements OnInit {

  animeModel: MAL_AnimeModel | null = null;
  animeList: MAL_AnimeModel[] | null = null;
  loadingCount: number[] = [0,1,2,3,4];
  searchTerm: string | null = null;
  succeMessage: string | null = null;
  errorMessage: string | null = null;
  loading: boolean = true;
  addingLoading: boolean = false;

  classMediaType: string = "badge-success";

  constructor(private service: SearchByNameModalService, public modalRef: MdbModalRef<SearchByNameModalComponent>) { }

  ngOnInit(): void {
    //this.animeList = animeListMocked;

    this.getShowListByName(this.searchTerm!);
  }

  getShowListByName(searchTerm: string) {
    var searchTermJoined = searchTerm?.replace(" ", "%20");

    this.service.getShowListByName(searchTermJoined!)
      .subscribe(
        (data: MAL_AnimeModel[]) => {
          this.animeList = data;
          setInterval(() => this.loading = false, 100)
        },
        error => {
          this.errorMessage = error.error;
          this.loading = false;
        }
      );
  }

  postAddToNotion(show: MAL_AnimeModel) {
    this.addingLoading = true;

    this.service.postAddToNotion(show)
      .subscribe(
        () => {
          Notify.success(show.title + " correctly sent to Notion");
          this.addingLoading = false;
        },
        error => {
          this.errorMessage = error.error;
          console.log(this.errorMessage);
          this.addingLoading = false;
        }
    )
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

  remapBadgeColor(media_type: string) {
    var result: string = "badge-success";

    switch (media_type) {
      case "tv":
      default:
        result = "badge-success";
        break;
      case "movie":
        result = "badge-warning";
        break;
      case "ova":
        result = "badge-primary";        
        break;
      case "special":
        result = "badge-secondary";
        break;
    }

    return result;
  }

}
