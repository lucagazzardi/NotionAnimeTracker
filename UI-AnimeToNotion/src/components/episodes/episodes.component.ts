import { Component, Input, OnInit } from '@angular/core';
import { ToasterService } from 'gazza-toaster';
import { Observable, tap } from 'rxjs';
import { IAnimeEpisode } from '../../model/IAnimeEpisode';
import { InternalService } from '../../services/internal/internal.service';
import { opacityOnEnter, scaleUpOnEnter } from '../../assets/animations/animations';
import { transition, trigger, useAnimation } from '@angular/animations';

@Component({
  selector: 'app-episodes',
  templateUrl: './episodes.component.html',
  styleUrls: ['./episodes.component.scss'],
  animations: [
    trigger('opacityOnEnter', [
      transition(':enter', [
        useAnimation(opacityOnEnter)
      ])
    ]),
    trigger('scaleUpOnEnter', [
      transition(':enter', [
        useAnimation(scaleUpOnEnter)
      ])
    ]),
  ]
})
export class EpisodesComponent implements OnInit {

  episodesList$!: Observable<IAnimeEpisode[]>;

  @Input() malId: number = 0;

  loading: boolean = false;

  constructor(private internalService: InternalService, private toasterService: ToasterService) { }

  ngOnInit(): void {
    this.loadEpisodes();
  }

  /// Loads watched an not watched episodes
  loadEpisodes() {

    this.loading = true;
    this.episodesList$ = this.internalService.getAnimeEpisodes(this.malId)
      .pipe(tap(() => this.loading = false))
  }
}
