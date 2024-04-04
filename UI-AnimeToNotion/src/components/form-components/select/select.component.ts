import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_SELECT_CONFIG } from '@angular/material/select';
import { ANY } from '../../../model/form-model/AnyOption';
import { SelectItem } from '../../../model/form-model/SelectInterface';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  providers: [
    {
      provide: MAT_SELECT_CONFIG,
      useValue: { overlayPanelClass: ['mat-override-cdk'] },
    },
  ]
})
export class SelectComponent implements OnInit {

  rotatedIcon: boolean = false;
  valueSelected: boolean = false;
  internalValues: SelectItem[] = [];

  @Input() isFilter: boolean = true;
  @Input() values: SelectItem[] = [];
  @Input() initialValue: string | null = null;
  @Input() background: string = "background";

  status = new FormControl();
  @Output() valueChanged: EventEmitter<SelectItem | null> = new EventEmitter();

  panelClass: string = '';

  constructor() { }

  ngOnInit(): void {
    // Calls the OnChange to emit the selected value
    // If the value is not null (e.g. after a reset) set the value as selected and show Reset icon
    this.status.valueChanges.subscribe(x => {
      this.onChange(x);
      this.valueSelected = this.status.value != ANY.value;
    });
  }

  ngOnChanges(changes: SimpleChanges) {

    this.isFilter ? this.panelClass = 'filter-panel' : this.panelClass = '';

    if (this.internalValues.length == 0)
      this.isFilter ? this.internalValues = [ANY, ...this.values] : this.internalValues = [...this.values];

    changes["initialValue"].currentValue ? this.status.setValue(changes["initialValue"].currentValue) : this.status.setValue(ANY.value);
  }

  onChange(status: string) {
    if (status && status != ANY.value)
      this.valueChanged.emit({ value: status, viewValue: status })
    else
      this.valueChanged.emit(null);
  }

  rotateIcon(value: boolean) {
    this.rotatedIcon = value;
  }

  reset() {
    this.status.setValue(ANY.value);
  }

}
