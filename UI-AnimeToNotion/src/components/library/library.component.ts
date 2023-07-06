import { Options } from '@angular-slider/ngx-slider';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, concat, debounceTime, delay, distinctUntilChanged, map, Observable, of, switchMap, tap } from 'rxjs';
import { opacityOnEnter, scaleUpOnEnter, totalScaleDown_OpacityOnLeave, totalScaleUp_OpacityOnEnter, totalScaleUp_Opacity_MarginOnEnter, totalScaleUp_Opacity_MarginOnLeave, YMovement_Opacity, YMovement_Opacity_Leave } from '../../assets/animations/animations';
import { MAL_AnimeModel } from '../../model/MAL_AnimeModel';
import { SelectShowStatus } from '../../model/form-model/SelectShowStatus';
import { SelectYear } from '../../model/form-model/SelectYear';
import { SelectFormat } from '../../model/form-model/SelectFormat';
import { EditService } from '../../services/edit/edit.service';
import { MalService } from '../../services/mal/mal.service';
import { InternalService } from '../../services/notion/internal.service';
import { IAnimeFull } from '../../model/IAnimeFull';
import { IAnimeBase } from '../../model/IAnimeBase';

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

  libraryList$!: Observable<IAnimeBase[]>;
  libraryListStatic!: IAnimeBase[];
  libraryListTracker!: boolean[];

  //! GENERAL FIELDS
  filterBackground: string = "foreground";
  showSliders: boolean = false;

  //! SEARCH RESULTS
  searchTerm: string = "";
  private searchTerm$ = new BehaviorSubject<string>('');
  searchResult$!: Observable<{ loading: boolean, list?: IAnimeBase[] }>;
  searchResultTracker!: boolean[];
  searching: boolean = false;
  noResults: boolean = false;


  constructor(
    private malService: MalService,
    private notionService: InternalService,
    private editService: EditService
  ) { }

  ngOnInit(): void {
    this.libraryList$ = this.malService.getAll().pipe(delay(1000));
    this.libraryList$.subscribe((data) => { this.libraryListStatic = data; this.libraryListTracker = Array(data.length).fill(false) });
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

  /// Debouncing for the search function
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

  ///API call for searching
  querySearch(searchTerm: string) {
    return this.notionService.search(searchTerm);
  }

  /// Open Edit Component
  editItem(item: IAnimeBase) {
    this.editService.editItem(item);
  }

  /// Show sliders
  switchShowSliders() {
    this.showSliders = !this.showSliders;
  }

}
