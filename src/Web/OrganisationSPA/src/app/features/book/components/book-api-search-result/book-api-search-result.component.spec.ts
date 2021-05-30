import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookApiSearchResultComponent } from './book-api-search-result.component';

describe('BookApiSearchResultComponent', () => {
  let component: BookApiSearchResultComponent;
  let fixture: ComponentFixture<BookApiSearchResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookApiSearchResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookApiSearchResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
