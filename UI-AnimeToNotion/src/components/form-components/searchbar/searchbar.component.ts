import { Component, ElementRef, EventEmitter, Input, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.scss']
})
export class SearchbarComponent implements OnInit {

  @Output() valueChanged: EventEmitter<string> = new EventEmitter();
  @Input() initialValue: string = "";

  @ViewChild('input') input: ElementRef

  constructor() { }

  ngOnInit(): void {
    
  }

  ngOnChanges(changes: SimpleChanges) {
  }

  onChange(value: any) {
    this.valueChanged.emit(value.target.value);
  }

  resetValue() {
    this.input.nativeElement.value = '';
  }

}
