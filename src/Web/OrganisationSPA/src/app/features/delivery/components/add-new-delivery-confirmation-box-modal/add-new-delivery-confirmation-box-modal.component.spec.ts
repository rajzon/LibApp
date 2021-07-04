import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewDeliveryConfirmationBoxModalComponent } from './add-new-delivery-confirmation-box-modal.component';

describe('AddNewDeliveryConfirmationBoxModalComponent', () => {
  let component: AddNewDeliveryConfirmationBoxModalComponent;
  let fixture: ComponentFixture<AddNewDeliveryConfirmationBoxModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddNewDeliveryConfirmationBoxModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddNewDeliveryConfirmationBoxModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
