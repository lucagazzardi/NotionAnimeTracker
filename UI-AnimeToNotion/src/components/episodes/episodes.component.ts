import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { ToasterService } from 'gazza-toaster';
import { Observable, tap } from 'rxjs';
import { AnimeEpisode, IAnimeEpisodesRecord, IAnimeSingleEpisode } from '../../model/IAnimeEpisode';
import { InternalService } from '../../services/internal/internal.service';
import * as _moment from 'moment';

@Component({
  selector: 'app-episodes',
  templateUrl: './episodes.component.html',
  styleUrls: ['./episodes.component.scss']
})
export class EpisodesComponent implements OnInit {

  episodesList$!: Observable<IAnimeEpisodesRecord>;

  datesFormControls: FormControl[] = [];
  @Input() id: string = '';
  @Input() malId: number = 0;

  animeShowId: string = '';

  loading: boolean = false;

  constructor(private internalService: InternalService, private toasterService: ToasterService) { }

  ngOnInit(): void {
    this.loadEpisodes();
  }

  /// Loads watched an not watched episodes
  loadEpisodes() {

    this.loading = true;
    this.episodesList$ = this.internalService.getAnimeEpisodes(this.id, this.malId)
      .pipe(tap(value => {
        this.animeShowId = value.animeShowId;
        value.episodes.forEach(x => this.datesFormControls.push(new FormControl(x.watchedOn ?? this.momentStartDay())));
        this.loading = false;
      }))
  }

  /// Adds or updates 
  addOrUpdateEpisode(episode: IAnimeSingleEpisode, index: number) {

    if (this.loading)
      return;

    // If there are no changes
    if (episode.watchedOn != null && episode.watchedOn == this.datesFormControls[index].value) {
      this.toasterService.notifySuccess('Episode has been updated');
      return;
    }      

    this.loading = true;

    // Update
    if (episode.watchedOn !== null) {
      this.internalService.editAnimeEpisode(new AnimeEpisode(episode.episodeId, this.animeShowId, episode.episodeNumber, this.datesFormControls[index].value))
        .subscribe(
          {
            next: () => {
              episode.watchedOn = this.datesFormControls[index].value;
              this.toasterService.notifySuccess('Episode has been updated');
              this.loading = false;
            },
            error: () => { this.toasterService.notifyError("Could not update episode"); this.loading = false; }
          });
    }
    // Add
    else {
      this.internalService.addAnimeEpisode(new AnimeEpisode(null, this.animeShowId, episode.episodeNumber, this.datesFormControls[index].value))
        .subscribe(
          {
            next: (data: any) => {
              episode.watchedOn = this.datesFormControls[index].value;
              episode.episodeId = data;
              this.toasterService.notifySuccess('Episode has been saved');
              this.loading = false;
            },
            error: () => { this.toasterService.notifyError("Could not save episode"); this.loading = false; }
          });
    }
  }

  /// Deletes watched anime episode
  deleteEpisode(episode: IAnimeSingleEpisode, index: number) {

    if (this.loading)
      return;

    if (episode.watchedOn == null)
      return;

    this.loading = true;

    this.internalService.deleteAnimeEpisode(episode.episodeId)
      .subscribe(
        {
          next: () => {
            episode.watchedOn = null;
            this.datesFormControls[index].setValue(this.momentStartDay());
            this.toasterService.notifySuccess('Episode has been deleted');
            this.loading = false;
          },
          error: () => { this.toasterService.notifyError("Could not delete episode"); this.loading = false; }
        });
  }

  /// Set Moment UTC at today start
  momentStartDay() {
    return _moment.utc().startOf('day');
  }
}
