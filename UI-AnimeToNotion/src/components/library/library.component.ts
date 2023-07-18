import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, map, Observable, of, switchMap, tap } from 'rxjs';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave, YMovement_Opacity, YMovement_Opacity_Leave } from '../../assets/animations/animations';
import { SelectShowStatus } from '../../model/form-model/SelectShowStatus';
import { SelectYear } from '../../model/form-model/SelectYear';
import { SelectFormat } from '../../model/form-model/SelectFormat';
import { EditService } from '../../services/edit/edit.service';
import { InternalService } from '../../services/internal/internal.service';
import { IAnimeBase } from '../../model/IAnimeBase';
import { IFilter, IPage, IQuery, Sort } from '../../model/IQuery';
import { ILibrary, IPageInfo } from '../../model/ILibrary';
import { IAnimeFull } from '../../model/IAnimeFull';
import { ToasterService } from 'gazza-toaster';

export interface DialogData {
  animal: string;
  name: string;
}

@Component({
  selector: 'app-library',
  templateUrl: './library.component.html',
  styleUrls: ['./library.component.scss'],
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
    trigger('totalScale_OpacityOnEnter', [
      transition(':enter', [
        useAnimation(totalScaleUp_OpacityOnEnter)
      ]),
      transition(':leave', [
        useAnimation(totalScaleDown_OpacityOnLeave)
      ])
    ]),
    trigger('totalScaleUp_Opacity_MarginOnEnter', [
      transition(':enter', [
        useAnimation(totalScaleUp_Opacity_MarginOnEnter)
      ]),
      transition(':leave', [
        useAnimation(totalScaleUp_Opacity_MarginOnLeave)
      ])
    ]),
    trigger('YMovement_Opacity', [
      transition(':enter', [
        useAnimation(YMovement_Opacity)
      ]),
      transition(':leave', [
        useAnimation(YMovement_Opacity_Leave)
      ]),
    ])
  ]
})
export class LibraryComponent implements OnInit {

  //! FILTER POPULATION
  showStatuses = SelectShowStatus;
  selectYear = SelectYear;
  selectFormat = SelectFormat;

  //! CARDS FIELDS
  loadingSkeleton = Array(20).fill(0);
  showImmersionButton: boolean = false;

  libraryList$!: Observable<ILibrary>;
  libraryListStatic!: IAnimeFull[];
  libraryListTracker!: boolean[];

  //! GENERAL FIELDS
  filterBackground: string = "foreground";
  showSliders: boolean = false;

  //! SEARCH RESULTS
  searchTerm: string = "";
  private searchTerm$ = new BehaviorSubject<string>('');
  searchResult$!: Observable<{ loading: boolean, result?: ILibrary }>;  
  searchResultTracker!: boolean[];
  searching: boolean = false;
  noResults: boolean = false;

  //! SELECTED FILTERS
  selectedYear: string | null = null;
  selectedStatus: string | null = null;
  selectedFormat: string | null = null;
  selectedFavoritesOnly: boolean | null = null;
  selectedMalScoreGreater: number | null = null;
  selectedMalScoreLesser: number | null = null;
  selectedPersonalScoreGreater: number | null = null;
  selectedPersonalScoreLesser: number | null = null;

  //! SORT
  selectedSort: string = 'Status';

  //! QUERY
  filters: IFilter = {} as IFilter;
  sort: Sort = Sort.Status;
  page: IPage = { currentPage: 1 } as IPage;

  initialValue: string | null = null;

  loading: boolean = false;

  constructor(
    private internalService: InternalService,
    private editService: EditService,
    private toasterService: ToasterService
  ) { }

  ngOnInit(): void {
    this.libraryQuery();
  }

  /// Triggered when something is typed in the search bar
  search(searchTerm: string) {
    this.resetPage();
    this.searchTerm$.next(searchTerm);
    this.debouncingPipe();
    this.noResults = false;
  }

  /// Debouncing for the search function
  debouncingPipe() {
    this.libraryList$ = this.searchTerm$
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        switchMap(searchTerm =>
          concat(
            // Starts with
            of({ data: [], pageInfo: {} as IPageInfo }).pipe(tap(value => this.loading = true)),
            // API call
            this.querySearch(searchTerm)
              .pipe(
                // Result
                tap(value => {
                  this.noResults = value.data.length == 0;
                  this.libraryListTracker = Array(value.data.length).fill(false);
                  this.libraryListStatic = value.data;
                  this.loading = false;
                }),
                map(value => ({ data: value.data, pageInfo: value.pageInfo })),

              )
          )
        )
      );
  }

  ///API call for searching
  querySearch(searchTerm: string) {
    this.setFilter("search", searchTerm, false);
    return this.internalService.libraryQuery({ filters: this.filters, sort: this.sort, page: this.page });
  }

  /// Open Edit Component
  editItem(item: IAnimeBase) {
    this.editService.editItem(item);
  }

  /// Add to plan to watch
  setPlanToWatch(item: IAnimeFull) {
    this.internalService.setPlanToWatch(item.info?.id!, !item.planToWatch)
      .subscribe(
        {
          next: (data: boolean) => {
            item!.planToWatch = data;
            let message = data ? " planned to watch" : " removed from planning";
            this.toasterService.notifySuccess(item!.nameEnglish + message);
          },
          error: () => { this.toasterService.notifyError("The entry could not be updated") }
        });
  }

  /// Show sliders
  switchShowSliders() {
    this.showSliders = !this.showSliders;
  }

  /// Set filter for query
  setFilter(filter: string, value: any, call: boolean = true ) {

    switch (filter) {
      case "search":
        this.filters.search = value;
        break;

      case "status":
        this.filters.status = value?.value ?? null;
        break;

      case "format":
        this.filters.format = value?.value ?? null;
        break;

      case "year":
        this.filters.year = value?.value ?? null;
        break;

      case "malScore":

        if (value.value == 0 && value.highValue == 100) {
          this.filters.malScoreGreater = null;
          this.filters.malScoreLesser = null;
          break;
        }

        this.filters.malScoreGreater = value.value;
        this.filters.malScoreLesser = value.highValue;
        break;

      case "personalScore":

        if (value.value == 0 && value.highValue == 100) {
          this.filters.personalScoreGreater = null;
          this.filters.personalScoreLesser = null;
          break;
        }

        this.filters.personalScoreGreater = value.value;
        this.filters.personalScoreLesser = value.highValue;
        break;

      case "episodes":
        this.filters.episodesGreater = value.value;
        this.filters.episodesLesser = value.highValue;
        break;

      case "favoritesOnly":
        if (value)
          this.filters.favoritesOnly = value;
        else
          this.filters.favoritesOnly = null;
        break;

      case "plannedOnly":
        if (value)
          this.filters.planToWatchOnly = value;
        else
          this.filters.planToWatchOnly = null;
        break;
    }

    this.page.currentPage = 1;

    if (call)
      this.libraryQuery();

  }

  /// Retrieve animes from library based on filter and sort
  libraryQuery() {
    this.noResults = false;
    this.libraryListStatic = [];
    this.libraryList$ = this.internalService.libraryQuery({ filters: this.filters, sort: this.sort, page: this.page })
      .pipe(tap(value => {
        this.noResults = value.data.length == 0;
        this.libraryListStatic = value.data;
        this.libraryListTracker = Array(value.data.length).fill(false);
      }));
  }

  /// Retrieve the following page based on the current query filters and sort
  libraryFollowingPage() {
    this.loading = true;
    this.internalService.libraryQuery({ filters: this.filters, sort: this.sort, page: this.page })
      .subscribe({
        next: query => this.libraryListStatic = [...this.libraryListStatic, ...query.data],
        error: err => this.toasterService.notifyError("Could not retrieve following page"),
        complete: () => this.loading = false
      })
  }

  /// Called from html on scroll
  followingPage() {
    this.page.currentPage += 1;
    this.libraryFollowingPage();
  }

  /// Set page for query
  setPage(page: number) {
    this.page.currentPage = page;
  }

  /// Reset filters
  resetFilter() {
    this.filters = {} as IFilter;
  }

  /// Reset pages after change of filters/sort
  resetPage() {
    this.libraryListStatic = [];
    this.page.currentPage = 1;
  }

  /// Set sort for query
  setSort(sort: string) {
    this.page.currentPage = 1;
    this.sort = <Sort>sort;

    this.libraryQuery();
  } 

}
