import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { SelectSort } from '../../model/form-model/SelectSort';

@Component({
  selector: 'app-sort',
  templateUrl: './sort.component.html',
  styleUrls: ['./sort.component.scss']
})
export class SortComponent implements OnInit {

  favoritesLabel: string = "Show Favorites Only";
  planToWatchLabel: string = "Show Planned Only";
  selectSort: string[] = SelectSort;

  selectedValue: string = 'Status';
  selectedValueFavorites: boolean = false;
  selectedValuePlanToWatch: boolean = false;
  @Output() valueChanged: EventEmitter<string> = new EventEmitter();
  @Output() valueChangedFavorites: EventEmitter<boolean> = new EventEmitter();
  @Output() valueChangedPlanToWatch: EventEmitter<boolean> = new EventEmitter(); 


  constructor() { }

  ngOnInit(): void {
  }

  onChange(value: string) {
    this.selectedValue = value;
    this.valueChanged.emit(value.replace(/\s/g, ''));
  }

  onChangeFavorites(value: boolean) {
    this.selectedValueFavorites = value;
    this.valueChangedFavorites.emit(value);
  }

  onChangePlanToWatch(value: boolean) {
    this.selectedValuePlanToWatch = value;
    this.valueChangedPlanToWatch.emit(value);
  }

}
