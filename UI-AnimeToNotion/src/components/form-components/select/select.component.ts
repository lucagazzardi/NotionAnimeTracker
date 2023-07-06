import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_SELECT_CONFIG } from '@angular/material/select';
import { SelectItem } from '../../../model/form-model/SelectInterface';
import { SelectShowStatus } from '../../../model/form-model/SelectShowStatus';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  providers: [
    {
      provide: MAT_SELECT_CONFIG,
      useValue: { overlayPanelClass: 'mat-override-cdk' },
    },
  ]
})
export class SelectComponent implements OnInit {

  rotatedIcon: boolean = false;
  valueSelected: boolean = false;

  @Input() values: SelectItem[] = [];
  @Input() initialValue: string | null = null;
  @Input() background: string = "background";

  status = new FormControl();
  @Output() valueChanged: EventEmitter<SelectItem | null> = new EventEmitter();  

  constructor() { }

  ngOnInit(): void {
    // Calls the OnChange to emit the selected value
    // If the value is not null (e.g. after a reset) set the value as selected and show Reset icon
    this.status.valueChanges.subscribe(x => {
      this.onChange(x);
      this.valueSelected = this.status.value != null;
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    this.status.setValue(changes["initialValue"].currentValue);    
  }

  onChange(status: string) {
    if (status)
      this.valueChanged.emit({ value: status, viewValue: status })
    else
      this.valueChanged.emit(null);
  }

  rotateIcon(value: boolean) {
    this.rotatedIcon = value;
  }

  reset() {
    this.status.setValue(null);
  }

}
