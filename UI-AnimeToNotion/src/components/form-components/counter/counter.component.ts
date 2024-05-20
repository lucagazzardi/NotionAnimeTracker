import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { CounterType } from '../../../model/form-model/CounterType';

@Component({
  selector: 'app-counter',
  templateUrl: './counter.component.html',
  styleUrls: ['./counter.component.scss']
})
export class CounterComponent implements OnInit {

  @Input() initialValue: number | null = null;
  @Input() maxValue: number | null = null;
  @Input() minValue: number | null = null;
  @Input() type: CounterType | null = null;

  types = CounterType;

  iconColor: string = 'rgb(var(--color-text-normal))';

  newValue = new FormControl(this.initialValue, [Validators.min(0), Validators.max(100)]);
  @Output() valueChanged: EventEmitter<number> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
    // Automatically handles value over the limits
    this.newValue.valueChanges.subscribe({
      next: (value) => {
        if (this.maxValue != null && value > this.maxValue)
          this.newValue.setValue(this.maxValue);
        else if (this.minValue != null && value < this.minValue)
          this.newValue.setValue(this.minValue);

        this.onChange(value);
      }
    })

  }

  ngOnChanges(changes: SimpleChanges) {
    this.newValue.setValue(changes["initialValue"].currentValue);    
  }

  upVote() {
    if (this.newValue.value == this.maxValue)
      return;

    this.newValue.setValue(this.newValue.value + 1);
  }

  downVote() {
    if (this.newValue.value == this.minValue)
      return;

    this.newValue.setValue(this.newValue.value - 1);
  }

  onChange(value: number) {
    this.valueChanged.emit(value);
  }
}
