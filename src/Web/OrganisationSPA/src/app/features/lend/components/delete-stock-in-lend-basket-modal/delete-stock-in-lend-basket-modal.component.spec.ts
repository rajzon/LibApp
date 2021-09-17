import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteStockInLendBasketModalComponent } from './delete-stock-in-lend-basket-modal.component';

describe('DeleteStockInLendBasketModalComponent', () => {
  let component: DeleteStockInLendBasketModalComponent;
  let fixture: ComponentFixture<DeleteStockInLendBasketModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteStockInLendBasketModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteStockInLendBasketModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
