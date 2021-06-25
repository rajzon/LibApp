import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeliveryPendingListComponent } from './delivery-pending-list.component';

describe('DeliveryPendingListComponent', () => {
  let component: DeliveryPendingListComponent;
  let fixture: ComponentFixture<DeliveryPendingListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeliveryPendingListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeliveryPendingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
