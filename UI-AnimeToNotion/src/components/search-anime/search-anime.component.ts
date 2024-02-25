import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, ElementRef, HostListener, OnInit, QueryList, ViewChildren } from '@angular/core';
import { BehaviorSubject, concat, debounceTime, distinctUntilChanged, map, of, switchMap, tap, toArray, Observable, Subscription, filter, combineLatest } from 'rxjs';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave } from '../../assets/animations/animations';
import { InternalService } from '../../services/internal/internal.service';
import { ToasterService } from 'gazza-toaster';
import { EditService } from '../../services/edit/edit.service';
import { MalService } from '../../services/mal/mal.service';
import { IAnimeBase } from '../../model/IAnimeBase';
import { IAnimePersonal } from '../../model/IAnimePersonal';
import { IAnimeFull } from '../../model/IAnimeFull';
import { ActivatedRoute, NavigationStart, Router } from '@angular/router';


@Component({
  selector: 'app-search-anime',
  templateUrl: './search-anime.component.html',
  styleUrls: ['./search-anime.component.scss'],
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
      transition(':leave', [
        useAnimation(totalScaleDown_OpacityOnLeave)
      ]),
      transition(':enter', [
        useAnimation(totalScaleUp_OpacityOnEnter)
      ])      
    ]),
    trigger('totalScaleUp_Opacity_MarginOnEnter', [
      transition(':leave', [
        useAnimation(totalScaleUp_Opacity_MarginOnLeave)
      ]),
      transition(':enter', [
        useAnimation(totalScaleUp_Opacity_MarginOnEnter)
      ])
    ])
  ]
})
export class SearchAnimeComponent implements OnInit {

  //! ==General fields==
  searchById: boolean = false;
  searchMode: string = "Title";
  seasonalSkeleton = Array(12).fill(0);
  showEditButton: boolean = false;
  suppressEditAnim: boolean = false;

  //! ==SEASONAL==
  seasonalList$!: Observable<IAnimeBase[]>;
  seasonalListStatic!: IAnimeBase[];
  seasonalListTracker!: boolean[];
  seasonalListImages!: boolean[];

  //! ==NEXT SEASON==
  nextSeasonList$!: Observable<IAnimeBase[]>;
  nextSeasonListStatic!: IAnimeBase[];
  nextSeasonTracker!: boolean[];
  nextSeasonImages!: boolean[];


  //! ==SEARCH==
  searchTerm: string = "";
  popStateNavigation: boolean = false;
  private searchTerm$ = new BehaviorSubject<string>('');
  searchResult$!: Observable<{ loading: boolean, list?: IAnimeBase[] }>;
  searchResultTracker!: boolean[];
  searchResultImages!: boolean[];
  searching: boolean = false;
  noResults: boolean = false;

  @ViewChildren("hover") hover: QueryList<ElementRef> = new QueryList<ElementRef>();

  private _routerSub = Subscription.EMPTY;

  constructor(
    private malService: MalService,
    private internalService: InternalService,
    private toasterService: ToasterService,
    private editService: EditService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {

    /// Defines if page is loading from a back navigation with a searched term or not
    this.loadInitialData();
    
    //! DEBOUNCING START
    this.debouncingPipe();
  }

  ngAfterViewInit(): void {
    this.hover.changes.subscribe((x: QueryList<ElementRef>) => {
      x.toArray().forEach(x => this.isInViewport(x))
    });
  }

  ngOnDestroy() {
    this._routerSub.unsubscribe();
  }

  loadInitialData() {       

    // The following subscribes are executed in this order by Angular

    /// Gets all the NavigationStart events and if they are from a popstate trigger (back and forward buttons) reloads library
    this._routerSub = this.router.events
      .pipe(
        filter(
          event => {
            return (event instanceof NavigationStart);
          }
        )
      )
      .subscribe((x: any) => {
        if (x.navigationTrigger == 'popstate') {
          this.popStateNavigation = true;
        }
      });

    /// Updates the search term everytime it changes from query parameters and defines which kind of data to load
    this.activatedRoute.queryParamMap.subscribe(x => {

      let search = x.get('search') ?? '';
      this.searchTerm = search;

      // If coming from a back or next navigation
      if (this.popStateNavigation && search) this.search(search);
      else if (this.popStateNavigation && !this.seasonalListStatic)
      {
        this.searching = false;
        this.loadCurrentAndUpcomingSeasons();
      }
      else if (this.popStateNavigation && this.seasonalListStatic) this.search(search);

    });    

    // If component is loaded with search filter in query param, search instead of loading current and upcoming seasons
    if (this.searchTerm)
      this.search(this.searchTerm)    
    else this.loadCurrentAndUpcomingSeasons();
  }

  loadCurrentAndUpcomingSeasons() {
    this.seasonalList$ = this.malService.getCurrentSeason()
      .pipe(tap(value => { this.seasonalListStatic = value; this.seasonalListTracker = Array(value.length).fill(false), this.seasonalListImages = Array(value.length).fill(false) }));

    this.nextSeasonList$ = this.malService.getUpcomingSeason()
      .pipe(tap(value => { this.nextSeasonListStatic = value; this.nextSeasonTracker = Array(value.length).fill(false), this.nextSeasonImages = Array(value.length).fill(false) }));

  }

  /// Alternate search by id or by title
  switchMode() {
    if (this.searchMode === "Mal ID") this.searchMode = "Title";
    else this.searchMode = "Mal ID";
  }

  /// Triggered when something is typed in the search bar
  search(searchTerm: string) {
    if (searchTerm) {
      this.searchTerm$.next(searchTerm);
      this.searching = true;
      this.noResults = false;
    }
    else {
      this.updateQueryParams();
      this.searching = false;
      if (!this.seasonalListStatic) this.loadCurrentAndUpcomingSeasons();
    }
      
  }

  /// Retrieve the value of text typed in search bar
  getEventType(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }

  /// Set or remove query params from route
  updateQueryParams(name: string | null = null, value: string | null = null) {

    if (!name)
      this.router.navigate([],
        {
          relativeTo: this.activatedRoute,
          queryParams: {}
        })
    else {
      this.router.navigate([],
        {
          relativeTo: this.activatedRoute,
          queryParams: { [name]: value }
        })
    }
  } 

  /// Triggered if there's no input for more than [debounceTime] ms in the search bar
  debouncingPipe() {
    this.searchResult$ = this.searchTerm$
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        switchMap(searchTerm =>
          concat(
            // Starts with
            of({ loading: true, list: [] }).pipe(tap(value => {              
              searchTerm === '' ? this.updateQueryParams() : this.updateQueryParams('search', searchTerm);
            })),
            // API call
            this.querySearch(searchTerm)
              .pipe(
                // Result
                tap(value => {
                  this.noResults = value.length == 0;
                  this.searchResultTracker = Array(value.length).fill(false);
                  this.searchResultImages = Array(value.length).fill(false);
                  this.updateQueryParams('search', searchTerm);
                }),
                map(value => ({ loading: false, list: value })),
                
              )
          )
        )
    );
  }

  /// Calls the right API based on the type of search being executed (ID / TITLE)
  querySearch(searchTerm: string) {
    if (this.searchMode === "Mal ID") {
      return this.malService.getShowFullById(searchTerm).pipe(toArray());
    }
    else {
      return this.malService.searchByName(searchTerm);
    }    
  }

  /// API call to add a new anime
  addBaseItem(newItem: IAnimeBase) {
    this.internalService.addBase(newItem)
      .subscribe(
        {
          next: (data: IAnimePersonal) => { newItem.info = data, this.toasterService.notifySuccess((newItem.nameEnglish) + " has been saved") },
          error: (error) => { this.toasterService.notifyError(error.error) }
        });
  }

  /// API call to remove an anime
  removeItem(item: IAnimeBase) {
    this.internalService.remove(item.info!.id)
      .subscribe(
        {
          next: () => { item.info = null; this.toasterService.notifySuccess((item.nameEnglish) + " has been removed") },
          error: (error) => { this.toasterService.notifyError(error.error) }
        });      ;
  }

  /// Open Edit Component
  editItem(item: IAnimeBase | IAnimeFull) {
    this.editService.editItem(item);
  }

  isInViewport(item: ElementRef) {
    const rect = item.nativeElement.getBoundingClientRect();
    if (item.nativeElement.classList.contains('show-visualization__show-hover--right') && rect.right > (window.innerWidth || document.documentElement.clientWidth)) {
      item.nativeElement.classList.remove('show-visualization__show-hover--right');
      item.nativeElement.classList.add('show-visualization__show-hover--left');
    }
    if (item.nativeElement.classList.contains('show-visualization__show-hover--left') && rect.left < 0) {
      item.nativeElement.classList.remove('show-visualization__show-hover--left');
      item.nativeElement.classList.add('show-visualization__show-hover--right');
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.hover.toArray().forEach(x => this.isInViewport(x));
  }
}
