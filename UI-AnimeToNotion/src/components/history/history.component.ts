import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IHistory } from '../../model/IHistory';
import { InternalService } from '../../services/internal/internal.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {

  historyYears$!: Observable<IHistory[]>

  constructor(private internalService: InternalService) { }

  ngOnInit(): void {
    this.historyYears$ = this.internalService.getHistory();
  }

}
