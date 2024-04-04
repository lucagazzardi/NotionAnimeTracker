import { transition, trigger, useAnimation } from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, filter, last, map, Observable, of, Subscription, switchMap, take, tap } from 'rxjs';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave, YMovement_Opacity, YMovement_Opacity_Leave } from '../../assets/animations/animations';
import { SelectShowStatus } from '../../model/form-model/SelectShowStatus';
import { SelectYear } from '../../model/form-model/SelectYear';
import { SelectFormat } from '../../model/form-model/SelectFormat';
import { EditService } from '../../services/edit/edit.service';
import { InternalService } from '../../services/internal/internal.service';
import { IAnimeBase, IKeyValue } from '../../model/IAnimeBase';
import { IFilter, IPage, Sort } from '../../model/IQuery';
import { ILibrary, IPageInfo } from '../../model/ILibrary';
import { IAnimeFull } from '../../model/IAnimeFull';
import { ToasterService } from 'gazza-toaster';
import { ActivatedRoute, NavigationEnd, NavigationStart, Params, Router } from '@angular/router';

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
  genres: IKeyValue[];

  //! CARDS FIELDS
  loadingSkeleton = Array(20).fill(0);
  showImmersionButton: boolean = false;

  libraryList$!: Observable<ILibrary>;
  libraryListStatic!: IAnimeFull[];
  libraryListTracker!: boolean[];
  libraryListImages!: boolean[];

  authLink: string = '';

  //! GENERAL FIELDS
  filterBackground: string = "foreground";
  showAdditionalFilters: boolean = false;

  //! SEARCH RESULTS
  searchTerm: string = "";
  private searchTerm$ = new BehaviorSubject<string>('');
  searching: boolean = false;
  noResults: boolean = false;

  //! QUERY
  filters: IFilter = {} as IFilter;
  sort: Sort = Sort.Status;
  page: IPage = { currentPage: 1 } as IPage;

  initialValue: string | null = null;

  loading: boolean = false;

  fromPopState: boolean = false;

  private _routerSub = Subscription.EMPTY;

  constructor(
    private internalService: InternalService,
    private editService: EditService,
    private toasterService: ToasterService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadInitialData();
    this.loadGenres();
  }

  ngOnDestroy() {
    this._routerSub.unsubscribe();
  }

  /// Defines if page is loading from a back navigation with a searched term or not
  loadInitialData() {

    this.searchTerm = this.getSearchParam();

    /// Handles Navigation events
    this._routerSub = this.router.events
      .pipe(        
        filter(
          event => {
            return (event instanceof NavigationStart || event instanceof NavigationEnd);
          }
        )
      )
      .subscribe((x: any) => {        

        // NavigationStart: if popstate (back and forth buttons), track the event, update the search term (empty string otherwise) and reloads library
        if (x instanceof NavigationStart && x.navigationTrigger == 'popstate') {
          this.fromPopState = true;
          this.searchTerm = this.getSearchParam();
          this.libraryQuery();
        }
        // NavigationEnd: if from popstate, detect changes (sends to child searchbar the input text update) and reset popstate 
        if (x instanceof NavigationEnd && this.fromPopState) {
          this.cd.detectChanges();
          this.searchTerm = this.getSearchParam();
          this.fromPopState = false;
        }
        else if (x instanceof NavigationEnd) {
          this.searchTerm = this.getSearchParam();
          if (!this.searchTerm)
            this.libraryQuery();
        }

      });

    /// Updates filters everytime query parameters change
    this.activatedRoute.queryParamMap.subscribe(x => {      

      let search = x.get('search') ?? '';
      this.setFilter('search', search, false);
      
    });

    /// At reinitialize of this component, if from popstate event, this is called both in the subscribe above and here, but for some reason it doesn't make two calls so it's ok
    this.libraryQuery();

  }

  loadGenres() {
    this.internalService.getGenres().subscribe({
      next: (data: IKeyValue[]) => { this.genres = data },
      error: () => { this.toasterService.notifyError("Could not retrieve Genres") }
    });     
  }

  /// Triggered when something is typed in the search bar
  search(searchTerm: string) {    
    this.resetPage();
    this.searchTerm$.next(searchTerm);
    this.debouncingPipe();
  }

  /// Debouncing for the search function
  debouncingPipe() {
    this.libraryList$ = this.searchTerm$
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(searchTerm =>
          concat(
            // Starts with
            of({ data: [], pageInfo: {} as IPageInfo }).pipe(tap(value => {
              this.loading = true;
              searchTerm === '' ? this.updateQueryParams() : this.updateQueryParams('search', searchTerm);
            })),
            // API call
            this.querySearch(searchTerm)
              .pipe(
                // Result
                tap(value => {
                  this.noResults = value.data.length == 0;
                  this.libraryListTracker = Array(value.data.length).fill(false);
                  this.libraryListImages = Array(value.data.length).fill(false);
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

  /// Set or remove query params from route
  updateQueryParams(name: string | null = null, value: string | null = null) {

    if (!name) {
      this.router.navigate([],
        {
          relativeTo: this.activatedRoute,
          queryParams: {}
        })
    }
    else {
      this.router.navigate([],
        {
          relativeTo: this.activatedRoute,
          queryParams: { [name]: value }
        })
    }
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
  switchShowAdditionalFilters() {
    this.showAdditionalFilters = !this.showAdditionalFilters;
  }

  /// Set filter for query
  setFilter(filter: string, value: any, call: boolean = true ) {

    switch (filter) {
      case "search":
        this.filters.search = value;
        break;

      case "genre":
        this.filters.genre = value;
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
    this.resetPage();
    this.libraryList$ = this.internalService.libraryQuery({ filters: this.filters, sort: this.sort, page: this.page })
      .pipe(tap(value => {
        this.noResults = value.data.length == 0;
        this.libraryListStatic = value.data;
        this.libraryListTracker = Array(value.data.length).fill(false);
        this.libraryListImages = Array(value.data.length).fill(false);
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
    this.libraryList$ = of({ data: [], pageInfo: {} as IPageInfo });
    this.page.currentPage = 1;
    this.noResults = false;
  }

  /// Change score color based on value
  getScoreColor(score: number | null) {

    if (score == null)
      return '';

    if (score > 70)
      return 'show-visualization__additionalinfo--score-good';
    else if (score > 50 && score <= 70)
      return 'show-visualization__additionalinfo--score-meh';
    else
      return 'show-visualization__additionalinfo--score-bad';
  }

  /// Get search term from queryparams
  getSearchParam() {
    return this.activatedRoute.snapshot.queryParamMap.get('search') ?? '';
  }

  /// Set sort for query
  setSort(sort: string) {
    this.page.currentPage = 1;
    this.sort = <Sort>sort;

    this.libraryQuery();
  } 

}
