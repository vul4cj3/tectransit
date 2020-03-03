import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingHeaderComponent } from './shipping-header.component';

describe('ShippingHeaderComponent', () => {
  let component: ShippingHeaderComponent;
  let fixture: ComponentFixture<ShippingHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
