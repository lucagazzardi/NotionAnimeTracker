import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.scss']
})
export class SearchbarComponent implements OnInit {

  @Output() valueChanged: EventEmitter<string> = new EventEmitter();
  @Input() initialValue: string = "";

  constructor() { }

  ngOnInit(): void {
    
  }

  ngOnChanges(changes: SimpleChanges) {
    
  }

  onChange(valuex: any) {
    this.valueChanged.emit(valuex.target.value);
  }

}
