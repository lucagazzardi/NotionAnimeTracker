import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.scss']
})
export class SearchbarComponent implements OnInit {

  searchTerm = new FormControl('');
  @Output() valueChanged: EventEmitter<string> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
    this.searchTerm.valueChanges.subscribe(x => this.onChange(x));
  }

  onChange(value: string) {
    this.valueChanged.emit(value);
  }

}
