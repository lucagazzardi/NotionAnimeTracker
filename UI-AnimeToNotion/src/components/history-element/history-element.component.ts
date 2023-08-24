import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'gazza-toaster';
import { Observable, tap } from 'rxjs';
import { scaleUpOnEnter } from '../../assets/animations/animations';
import { IAnimeFull } from '../../model/IAnimeFull';
import { ILibrary } from '../../model/ILibrary';
import { IYearCount } from '../../model/IYearCount';
import { EditService } from '../../services/edit/edit.service';
import { HistoryService } from '../../services/history/history.service';
import { InternalService } from '../../services/internal/internal.service';

@Component({
  selector: 'app-history-element',
  templateUrl: './history-element.component.html',
  styleUrls: ['./history-element.component.scss'],
  animations: [
    trigger('scaleUpOnEnter', [
      transition(':enter', [
        useAnimation(scaleUpOnEnter)
      ])
    ]),
  ]
})
export class HistoryElementComponent implements OnInit {

  year: string | null = null;
  yearList$!: Observable<ILibrary>;
  yearListStatic!: IAnimeFull[];
  yearListImages!: boolean[];

  yearSkeleton = Array(20).fill(0);
  noResults: boolean = false;

  currentPage: number = 1;
  loading: boolean = false;

  yearCount: IYearCount | null = null;

  constructor(
    private route: ActivatedRoute,
    private internalService: InternalService,
    private historyService: HistoryService,
    private toasterService: ToasterService,
    private editService: EditService
  ) { }

  ngOnInit(): void {
    // Retrieves year value from route
    this.year = this.route.snapshot.paramMap.get('year');

    // Retrieves the counts for watched and favorites
    this.retrieveCounts();

    // Retrieves the list of animes for the current year
    if (this.year != null)
      this.getYearList(this.year, this.currentPage);
  }

  /// Retrieves shows watched in year
  getYearList(year: string, page: number) {
    this.yearList$ = this.internalService.getHistoryYear(year, page).pipe(
      tap(value => {
        if (value.data.length == 0)
          this.noResults = true;
        this.yearListStatic = value.data;
        this.yearListImages = Array(value.data.length).fill(false)
      }));
  }

  /// Retrieves counts for watched and favorites
  getCounts(year: string) {
    this.internalService.getHistoryCounts(year)
      .subscribe(
        {
          next: (data: IYearCount) => { this.yearCount = data },
          error: () => { this.toasterService.notifyError("Year info could not be retrieved") }
        });
  }

  /// If year of the route is the same as the service, takes the counts. Retrieves from backend otherwise.
  retrieveCounts() {
    let serviceValue = this.historyService.getYear();

    if (serviceValue.year != this.year)
      this.getCounts(this.year!);
    else
      this.yearCount = { year: Number(this.year), completed: serviceValue!.completed!, favorite: serviceValue!.favorite! }    
  }  

  /// Open Edit Component
  editItem(item: IAnimeFull) {
    this.editService.editItem(item);
  }

  /// Retrieves and appends data pages after the first
  getFollowingPage(year: string, page: number) {
    this.loading = true;
    this.internalService.getHistoryYear(year, page)
      .subscribe({
        next: query => this.yearListStatic = [...this.yearListStatic, ...query.data],
        error: err => this.toasterService.notifyError("Could not retrieve following page"),
        complete: () => this.loading = false
      })
  }

  /// Retrieves the following page
  followingPage() {
    this.currentPage += 1;
    this.getFollowingPage(this.year!, this.currentPage);
  }

}
