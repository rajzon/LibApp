import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookManualAddComponent } from './book-manual-add.component';

describe('BookManualAddComponent', () => {
  let component: BookManualAddComponent;
  let fixture: ComponentFixture<BookManualAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookManualAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookManualAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
