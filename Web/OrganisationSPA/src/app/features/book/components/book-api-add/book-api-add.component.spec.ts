import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookApiAddComponent } from './book-api-add.component';

describe('BookApiAddComponent', () => {
  let component: BookApiAddComponent;
  let fixture: ComponentFixture<BookApiAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookApiAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookApiAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
