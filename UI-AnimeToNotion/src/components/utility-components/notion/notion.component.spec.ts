import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotionComponent } from './notion.component';

describe('NotionComponent', () => {
  let component: NotionComponent;
  let fixture: ComponentFixture<NotionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NotionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
