import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RedeemDeliveryComponent } from './redeem-delivery.component';

describe('RedeemDeliveryComponent', () => {
  let component: RedeemDeliveryComponent;
  let fixture: ComponentFixture<RedeemDeliveryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RedeemDeliveryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RedeemDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
