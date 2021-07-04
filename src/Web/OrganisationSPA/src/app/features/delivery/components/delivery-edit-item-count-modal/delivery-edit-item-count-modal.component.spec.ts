import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeliveryEditItemCountModalComponent } from './delivery-edit-item-count-modal.component';

describe('DeliveryEditItemCountModalComponent', () => {
  let component: DeliveryEditItemCountModalComponent;
  let fixture: ComponentFixture<DeliveryEditItemCountModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeliveryEditItemCountModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeliveryEditItemCountModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
