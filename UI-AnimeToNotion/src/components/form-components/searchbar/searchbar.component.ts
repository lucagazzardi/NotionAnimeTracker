import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.scss']
})
export class SearchbarComponent implements OnInit {

  searchTerm = new FormControl('');
  @Output() valueChanged: EventEmitter<string> = new EventEmitter();
  @Input() initialValue: string = "";

  constructor() { }

  ngOnInit(): void {
    this.searchTerm.valueChanges.subscribe(x => this.onChange(x));
  }

  ngOnChanges(changes: SimpleChanges) {
    this.searchTerm.setValue(changes["initialValue"].currentValue);
  }

  onChange(value: string) {
    this.valueChanged.emit(value);
  }

}
