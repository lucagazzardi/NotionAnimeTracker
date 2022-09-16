import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchByNameModalComponent } from './search-by-name-modal.component';

describe('SearchByNameModalComponent', () => {
  let component: SearchByNameModalComponent;
  let fixture: ComponentFixture<SearchByNameModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchByNameModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchByNameModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
