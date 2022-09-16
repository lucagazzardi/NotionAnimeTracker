import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchByIdModalComponent } from './search-by-id-modal.component';

describe('SearchByIdModalComponent', () => {
  let component: SearchByIdModalComponent;
  let fixture: ComponentFixture<SearchByIdModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchByIdModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchByIdModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
