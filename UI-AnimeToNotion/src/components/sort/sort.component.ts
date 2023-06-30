import { Options } from '@angular-slider/ngx-slider';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { SelectSort } from '../../model/form-model/SelectSort';

@Component({
  selector: 'app-sort',
  templateUrl: './sort.component.html',
  styleUrls: ['./sort.component.scss']
})
export class SortComponent implements OnInit {

  checkboxLabel: string = "Show Favorites Only";
  selectSort: string[] = SelectSort;

  selectedValue: string = 'Status';
  @Output() valueChanged: EventEmitter<string> = new EventEmitter(); 

  value: number = 100;
  options: Options = {
    floor: 0,
    ceil: 250
  };

  constructor() { }

  ngOnInit(): void {
  }

  onChange(value: string) {
    this.selectedValue = value;
    this.valueChanged.emit(value);
  }

}
