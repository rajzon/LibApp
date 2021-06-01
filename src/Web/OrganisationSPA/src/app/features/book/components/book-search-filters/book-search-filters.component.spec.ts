import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookSearchFiltersComponent } from './book-search-filters.component';

describe('BookSearchFiltersComponent', () => {
  let component: BookSearchFiltersComponent;
  let fixture: ComponentFixture<BookSearchFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookSearchFiltersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookSearchFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
