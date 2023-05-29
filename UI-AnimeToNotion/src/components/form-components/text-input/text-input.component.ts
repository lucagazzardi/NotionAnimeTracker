import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit {

  @Input() isTextArea: boolean = false;
  @Input() initialValue: string = '';

  newValue = new FormControl(this.initialValue);
  @Output() valueChanged: EventEmitter<string> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
    this.newValue.valueChanges.subscribe(x => this.onChange(x));
  }

  onChange(value: string) {
    this.valueChanged.emit(value);
  }

}
