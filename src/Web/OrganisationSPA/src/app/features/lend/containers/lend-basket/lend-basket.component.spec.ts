import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LendBasketComponent } from './lend-basket.component';

describe('LendBasketComponent', () => {
  let component: LendBasketComponent;
  let fixture: ComponentFixture<LendBasketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LendBasketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LendBasketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
