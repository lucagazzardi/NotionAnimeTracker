import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, EventEmitter, Input, Output, SimpleChange, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { map, startWith, tap } from 'rxjs/operators';
import { IKeyValue } from '../../../model/IAnimeBase';


@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.scss']
                
})
export class AutocompleteComponent {

  allItems: string[] | null;
  currentlySelectedValues: number[] = [];

  rotatedIcon: boolean = false;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  itemCtrl = new FormControl();
  filteredItems: Observable<string[]>;
  items: string[] = [];
  chipItems: string[] = [];  

  placeholder: string = '';
  chipsHidden: boolean = false;

  valueSelected: boolean = false;

  @Input() values: IKeyValue[] | null = [];
  @Input() background: string;

  @Output() valueChanged: EventEmitter<number[] | null> = new EventEmitter();

  @ViewChild('itemInput') itemInput: ElementRef<HTMLInputElement>;

  constructor() {
    this.filteredItems = this.itemCtrl.valueChanges.pipe(
      startWith(null),
      map((item: string | null) => (item ? this._filter(item) : this.allItems ? this.allItems.slice() : []))
    );
  }

  ngOnChanges() {
    this.allItems = this.values ? this.values?.map(x => x.value) : null;
    this.itemCtrl.setValue('');
    this.setPlaceHolder();
  }

  add(item: string): void {
    this.items.push(item);
    this.handleChipFruits();
    this.setPlaceHolder();
    this.itemCtrl.setValue(null);
  }

  remove(item: string): void {
    const itemsIndex = this.items.indexOf(item);

    if (itemsIndex >= 0) {
      this.items.splice(itemsIndex, 1);
      this.handleChipFruits();
      this.setPlaceHolder();
      this.itemCtrl.setValue(null);
    }
  }

  selected(event: Event, item:string): void {
    event.stopPropagation();

    const itemIndex = this.items.indexOf(item);
    this.itemInput.nativeElement.value = '';
    this.itemInput.nativeElement.blur();

    this._addOrRemove(itemIndex, item); 

    this.valueChanged.emit(this.currentlySelectedValues);
    this.valueSelected = this.items.length > 0;
  }

  setPlaceHolder() {
    if (this.items.length == 0)
      this.placeholder = 'Any';
    else
      this.placeholder = '';
  }

  hideChips() {
    this.chipsHidden = true;
  }

  showChips() {
    this.chipsHidden = false;
  }

  handleChipFruits() {
    if (this.items.length > 1) {
      this.chipItems = [this.items[0], '+' + (this.items.length - 1)];
    }
    else {
      this.chipItems = this.items;
    }
  }

  reset() {
    this.items = [];
    this.chipItems = [];
    this.setPlaceHolder();
    this.valueSelected = this.items.length > 0;
    this.currentlySelectedValues = [];
    this.valueChanged.emit(null);
  }

  rotateIcon(value: boolean) {
    this.rotatedIcon = value;
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allItems!.filter(item => item.toLowerCase().includes(filterValue));
  }

  private _addOrRemove(itemIndex: number, item: string) {
    let selected = this.values?.find(x => x.value == item)?.id!

    if (itemIndex < 0) {
      this.add(item);
      this.currentlySelectedValues.push(selected);
    }
    else {
      this.remove(item);
      const index = this.currentlySelectedValues.indexOf(selected);
      this.currentlySelectedValues.splice(index, 1);
    }
  }
}
