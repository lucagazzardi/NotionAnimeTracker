import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, map, of, switchMap, tap, toArray, Observable } from 'rxjs';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave } from '../../assets/animations/animations';
import { InternalService } from '../../services/internal/internal.service';
import { ToasterService } from 'gazza-toaster';
import { EditService } from '../../services/edit/edit.service';
import { MalService } from '../../services/mal/mal.service';
import { IAnimeBase } from '../../model/IAnimeBase';
import { IAnimePersonal } from '../../model/IAnimePersonal';
import { IAnimeFull } from '../../model/IAnimeFull';


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

  //! ==NEXT SEASON==
  nextSeasonList$!: Observable<IAnimeBase[]>;
  nextSeasonListStatic!: IAnimeBase[];
  nextSeasonTracker!: boolean[];

  //! ==SEARCH==
  searchTerm: string = "";
  private searchTerm$ = new BehaviorSubject<string>('');
  searchResult$!: Observable<{ loading: boolean, list?: IAnimeBase[] }>;
  searchResultTracker!: boolean[];
  searching: boolean = false;
  noResults: boolean = false;

  constructor(
    private malService: MalService,
    private internalService: InternalService,
    private toasterService: ToasterService,
    private editService: EditService
  ) { }

  ngOnInit(): void {

    this.seasonalList$ = this.malService.getCurrentSeason()
      .pipe(tap(value => { this.seasonalListStatic = value; this.seasonalListTracker = Array(value.length).fill(false) }));
    
    this.nextSeasonList$ = this.malService.getUpcomingSeason()
      .pipe(tap(value => { this.nextSeasonListStatic = value; this.nextSeasonTracker = Array(value.length).fill(false) }));

    //! DEBOUNCING START
    this.debouncingPipe();    
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
    else
      this.searching = false;
  }

  /// Retrieve the value of text typed in search bar
  getEventType(event: Event): string {
    return (event.target as HTMLInputElement).value;
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
            of({ loading: true, list: [] }),
            // API call
            this.querySearch(searchTerm)
              .pipe(
                // Result
                tap(value => {
                  this.noResults = value.length == 0;
                  this.searchResultTracker = Array(value.length).fill(false);
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
}
