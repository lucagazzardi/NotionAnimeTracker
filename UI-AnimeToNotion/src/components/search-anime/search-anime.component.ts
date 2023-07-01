import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, map, of, switchMap, tap, toArray, Observable } from 'rxjs';
import { MAL_AnimeModel } from '../../model/MAL_AnimeModel';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave } from '../../assets/animations/animations';
import { NotionService } from '../../services/notion/notion.service';
import { ToasterService } from 'gazza-toaster';
import { EditService } from '../../services/edit/edit.service';
import { MalService } from '../../services/mal/mal.service';
import { IAnimeBase } from '../../model/IAnimeBase';
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
    private notionService: NotionService,
    private toasterService: ToasterService,
    private editService: EditService
  ) { }

  ngOnInit(): void {

    this.seasonalList$ = this.malService.getCurrentSeason().pipe(tap(value => { this.seasonalListStatic = value; this.seasonalListTracker = Array(value.length).fill(false) }));
    
    this.nextSeasonList$ = this.malService.getUpcomingSeason().pipe(tap(value => { this.nextSeasonListStatic = value; this.nextSeasonTracker = Array(value.length).fill(false) }));

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

  /// API call to Notion (Add new item)
  addItemToNotion(newItem: MAL_AnimeModel) {
    this.notionService.add(newItem)
      .subscribe(
        {
          next: () => { this.toasterService.notifySuccess((newItem.alternative_titles.en ? newItem.alternative_titles.en : newItem.title) + " has been saved") },
          error: (error) => { this.toasterService.notifyError(error.error) }
        });
  }

  removeItemFromNotion(id: number) {
    // TODO: Add logic of removal
    this.notionService.remove(id);
  }

  /// Open Edit Component
  editItem(item: IAnimeBase) {
    this.editService.editItem(item);
  }
  
}
