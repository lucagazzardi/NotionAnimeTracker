import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { opacityOnEnter } from '../../assets/animations/animations';
import { IHistory } from '../../model/IHistory';
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
  skeletonYears = Array(6).fill(0);

  constructor(private internalService: InternalService) { }

  ngOnInit(): void {
    this.historyYears$ = this.internalService.getHistory();
  }

}
