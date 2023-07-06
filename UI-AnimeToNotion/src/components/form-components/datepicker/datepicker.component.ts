import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MAT_DATE_FORMATS} from '@angular/material/core';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';

const moment = _rollupMoment || _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL'
  },
};

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss'],
  providers: [
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class DatepickerComponent implements OnInit {

  @Input() initialValue: Date | null = null;

  date = new FormControl(this.initialValue);
  @Output() valueChanged: EventEmitter<Date> = new EventEmitter();

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.date.valueChanges.subscribe(x => this.onChange(x));
  }

  ngOnChanges(changes: SimpleChanges) {
    this.date.setValue(changes["initialValue"].currentValue);
  }

  onChange(value: Date) {
    this.valueChanged.emit(value);
  }

}
