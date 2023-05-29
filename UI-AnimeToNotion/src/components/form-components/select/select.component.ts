import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss']
})
export class SelectComponent implements OnInit {

  @Input() values: Map<number, string> = new Map<number, string>();
  @Input() initialValue: number | null = null;

  status = new FormControl(this.initialValue);
  @Output() valueChanged: EventEmitter<{ id: number, label: string }> = new EventEmitter();
  

  constructor() { }

  ngOnInit(): void {
    this.status.valueChanges.subscribe(x => this.onChange(x));
  }

  onChange(status: number) {
    this.valueChanged.emit({ id: status, label: this.values.get(status)! })
  }

}
