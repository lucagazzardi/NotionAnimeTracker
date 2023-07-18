import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss'],
  animations: [
    trigger('checkbox', [
      transition(':enter', [
        animate('150ms', keyframes([
          style({ opacity: 0, transform: 'scale(.4)' }),
          style({ opacity: 1, transform: 'scale(.8)' }),
          style({ opacity: 1, transform: 'none' }),
        ]))
      ]),
      transition(':leave', [
        animate('150ms', keyframes([
          style({ opacity: 1, transform: 'none' }),
          style({ opacity: 0, transform: 'scale(.6)' }),
          style({ opacity: 0, transform: 'scale(.4)' }),
        ]))
      ])
    ])
  ]

})
export class CheckboxComponent implements OnInit {

  checked: boolean = false;
  @Input() label: string = '';

  @Output() valueChanged: EventEmitter<boolean> = new EventEmitter();  

  constructor() { }

  ngOnInit(): void {
  }

  switchCheckbox() {
    this.checked = !this.checked;
    this.valueChanged.emit(this.checked);
  }
}
