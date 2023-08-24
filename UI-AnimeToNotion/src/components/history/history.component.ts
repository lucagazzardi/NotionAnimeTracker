import { transition, trigger, useAnimation } from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { opacityOnEnter } from '../../assets/animations/animations';
import { IAnimeFull } from '../../model/IAnimeFull';
import { IHistory } from '../../model/IHistory';
import { HistoryService } from '../../services/history/history.service';
import { InternalService } from '../../services/internal/internal.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss'],
  animations: [
    trigger('opacityOnEnter', [
      transition(':enter', [
        useAnimation(opacityOnEnter)
      ])
    ]),
  ]
})
export class HistoryComponent implements OnInit {

  historyYears$!: Observable<IHistory[]>;
  historyElements!: IAnimeFull[];
  historyImagesTracker!: boolean[];
  skeletonYears = Array(6).fill(0);

  constructor(private internalService: InternalService, private historyService: HistoryService) { }

  ngOnInit(): void {
    this.historyYears$ = this.internalService.getHistory();
  }

  /// Adds the loaded class when image loads
  loadImage(event: Event) {
    (event.target as HTMLImageElement).classList.add('year__image--loaded');
  }

  /// Set the year selected
  openYear(year: string, watched: number, favorite: number) {
    this.historyService.route(year, watched, favorite);
  }

}
