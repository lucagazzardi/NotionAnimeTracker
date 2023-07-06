import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.scss']
})
export class RankingComponent implements OnInit {

  @Input() initialValue: number | null = null;
  iconColor: string = 'rgb(var(--color-text-normal))';

  newValue = new FormControl(this.initialValue, [Validators.min(0), Validators.max(100)]);
  @Output() valueChanged: EventEmitter<number> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
    // Automatically handles value over the limits
    this.newValue.valueChanges.subscribe({
      next: (value) => {
        if (value > 100)
          this.newValue.setValue(100);
        else if (value < 0)
          this.newValue.setValue(0);

        this.onChange(value);
      }
    })

  }

  ngOnChanges(changes: SimpleChanges) {
    this.newValue.setValue(changes["initialValue"].currentValue);    
  }

  upVote() {
    if (this.newValue.value == 100)
      return;

    this.newValue.setValue(this.newValue.value + 1);
  }

  downVote() {
    if (this.newValue.value == 0)
      return;

    this.newValue.setValue(this.newValue.value - 1);
  }

  onChange(value: number) {
    this.valueChanged.emit(value);
  }
}
