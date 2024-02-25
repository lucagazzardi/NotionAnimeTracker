import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.scss']
})
export class SearchbarComponent implements OnInit {

  searchTerm = new FormControl('');
  @Output() valueChanged: EventEmitter<string> = new EventEmitter();
  @Input() initialValue: string = "";

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    
  }

  ngOnChanges(changes: SimpleChanges) {
    
  }

  onChange(valuex: any) {
    this.valueChanged.emit(valuex.target.value);
  }

}
