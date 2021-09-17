import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DumbInputTextComponent } from './dumb-input-text.component';

describe('DumbInputTextComponent', () => {
  let component: DumbInputTextComponent;
  let fixture: ComponentFixture<DumbInputTextComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DumbInputTextComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DumbInputTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
