import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookApiEditModalComponent } from './book-api-edit-modal.component';

describe('BookApiEditModalComponent', () => {
  let component: BookApiEditModalComponent;
  let fixture: ComponentFixture<BookApiEditModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookApiEditModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookApiEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
