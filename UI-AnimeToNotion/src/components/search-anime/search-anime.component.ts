import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, map, of, switchMap, tap, toArray, Observable } from 'rxjs';
import { MAL_AnimeModel } from '../../model/MAL_AnimeModel';
import { SearchAnimeService } from './search-anime.service';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave } from '../../assets/animations/animations';
import { NotionService } from '../../services/notion/notion.service';
import { ToasterService } from 'gazza-toaster';
import { Router } from '@angular/router';
import { StringManipulationService } from '../../services/string-manipulation/string-manipulation.service';
import { EditService } from '../edit/edit.service';


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
    ])
  ]
})
export class SearchAnimeComponent implements OnInit {

  //! ==General fields==
  searchById: boolean = false;
  seasonalSkeleton = Array(20).fill(0);
  showEditButton: boolean = false;

  //! ==SEASONAL==
  seasonalList$!: Observable<MAL_AnimeModel[]>;
  seasonalListStatic!: MAL_AnimeModel[];
  seasonalListTracker!: boolean[];

  //! ==NEXT SEASON==
  nextSeasonList$!: Observable<MAL_AnimeModel[]>;
  nextSeasonListStatic!: MAL_AnimeModel[];
  nextSeasonTracker!: boolean[];

  //! ==SEARCH==
  searchTerm: string = "";
  private searchTerm$ = new BehaviorSubject<string>('');
  searchResult$!: Observable<{ loading: boolean, list?: MAL_AnimeModel[] }>;
  searchResultTracker!: boolean[];
  searching: boolean = false;
  noResults: boolean = false;

  constructor(
    private router: Router,
    private service: SearchAnimeService,
    private notionService: NotionService,
    private toasterService: ToasterService,
    private stringManipulation: StringManipulationService,
    private editService: EditService
  ) { }

  ngOnInit(): void {

    // MOCK ITEMS
    this.seasonalList$ = this.service.getAll().pipe(delay(1000));
    this.seasonalList$.subscribe((data) => { this.seasonalListStatic = data; this.seasonalListTracker = Array(data.length).fill(false) });

    this.nextSeasonList$ = this.service.getAll().pipe(delay(1000));
    this.nextSeasonList$.subscribe((data) => { this.nextSeasonListStatic = data; this.nextSeasonTracker = Array(data.length).fill(false) });

    //! DEBOUNCING START
    this.debouncingPipe();    
  }

  /// Alternate search by id or by title
  switchMode(value: boolean) {
    this.searchById = value;
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
    if (this.searchById) {
      return this.service.getShowById(searchTerm).pipe(toArray());
    }
    else {
      return this.service.getShowListByName(searchTerm);
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
  editItem(item: MAL_AnimeModel) {
    let title: string = this.stringManipulation.normalize(item.alternative_titles.en ? item.alternative_titles.en : item.title);

    this.editService.setItem(item);
    this.router.navigate(['edit', item.id, title]);
  }





  testToast() {
    this.toasterService.notifyError("Tanto ciao dai è stato aggiunto con successo");
  }
  
}
