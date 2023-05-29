import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss']
})
export class DatepickerComponent implements OnInit {

  @Input() initialValue: Date | null = null;

  date = new FormControl(this.initialValue);
  @Output() valueChanged: EventEmitter<Date> = new EventEmitter();

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.date.valueChanges.subscribe(x => this.onChange(x));
  }

  onChange(value: Date) {
    this.valueChanged.emit(value);
  }

}
