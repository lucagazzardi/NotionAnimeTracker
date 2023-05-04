import { animate, keyframes, style, transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, map, Observable, of, startWith, Subject, Subscription, switchMap, takeUntil, tap, toArray } from 'rxjs';
import { MAL_AnimeModel } from '../../model/MAL_AnimeModel';
import { SearchByNameModalService } from '../../search-by-name-modal/search-by-name-modal.service';
import { SearchAnimeService } from './search-anime.service';
import { opacityOnEnter, scaleUpOnEnter } from '../utility-components/animations/animations';

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
  ]
})
export class SearchAnimeComponent implements OnInit {

  searchById: boolean = false;  
  seasonalList$!: Observable<MAL_AnimeModel[]>;
  seasonalListStatic!: MAL_AnimeModel[];
  seasonalSkeleton = Array(20).fill(0);

  searchTerm: string = "";
  private searchTerm$ = new BehaviorSubject<string>('');
  searchResult$!: Observable<{ loading: boolean, list?: MAL_AnimeModel[] }>;

  searching: boolean = false;
  noResults: boolean = false;

  constructor(private service: SearchAnimeService) { }

  ngOnInit(): void {

    //MOCK ITEMS
    this.seasonalList$ = this.service.getAll().pipe(delay(1000));
    this.seasonalList$.subscribe((data) => { this.seasonalListStatic = data });

    this.debouncingPipe();    
  }

  switchMode(value: boolean) {
    this.searchById = value;
  }

  search(searchTerm: string) {
    if (searchTerm) {
      this.searchTerm$.next(searchTerm);
      this.searching = true;
      this.noResults = false;
    }
    else
      this.searching = false;
  }

  getEventType(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }

  debouncingPipe() {
    this.searchResult$ = this.searchTerm$
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        switchMap(searchTerm =>
          concat(
            // Starts with
            of({ loading: true, list: [] }),
            // API call starts
            this.querySearch(searchTerm)
              .pipe(
                //Result
                tap(value => {
                  this.noResults = value.length == 0;
                }),
                map(value => ({ loading: false, list: value })),
                
              )
          )
        )
    );
  }

  querySearch(searchTerm: string) {
    if (this.searchById) {
      return this.service.getShowById(searchTerm).pipe(toArray());
    }
    else {
      return this.service.getShowListByName(searchTerm);
    }    
  }
}
