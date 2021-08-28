import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteDeliveryCanncellationReasonModalComponent } from './delete-delivery-canncellation-reason-modal.component';

describe('DeleteDeliveryCanncellationReasonModalComponent', () => {
  let component: DeleteDeliveryCanncellationReasonModalComponent;
  let fixture: ComponentFixture<DeleteDeliveryCanncellationReasonModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteDeliveryCanncellationReasonModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteDeliveryCanncellationReasonModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
