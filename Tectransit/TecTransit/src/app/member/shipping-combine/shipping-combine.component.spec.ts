import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingCombineComponent } from './shipping-combine.component';

describe('ShippingCombineComponent', () => {
  let component: ShippingCombineComponent;
  let fixture: ComponentFixture<ShippingCombineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingCombineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingCombineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
