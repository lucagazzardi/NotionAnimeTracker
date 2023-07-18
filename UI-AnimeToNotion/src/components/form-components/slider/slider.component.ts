import { ChangeContext, Options } from '@angular-slider/ngx-slider';
import { Component, EventEmitter, Inject, Input, OnInit, Output, QueryList, ViewChildren } from '@angular/core';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.scss']
})
export class SliderComponent implements OnInit {

  @Input() label: string = '';

  @Output() changeContext: EventEmitter<ChangeContext> = new EventEmitter();  

  minValue: number = 0;
  maxValue: number = 100;
  options: Options = {
    floor: 0,
    ceil: 100,
    step: 1
  };

  constructor() { }

  ngOnInit() {

  }

  onChange(changeContext: ChangeContext) {
    this.changeContext.emit(changeContext);
  }

}
