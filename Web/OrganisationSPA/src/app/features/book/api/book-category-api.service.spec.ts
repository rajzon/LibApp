import { TestBed } from '@angular/core/testing';

import { BookCategoryApiService } from './book-category-api.service';

describe('BookCategoryApiService', () => {
  let service: BookCategoryApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookCategoryApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
