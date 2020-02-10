import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingcusHeaderComponent } from './shippingcus-header.component';

describe('ShippingcusHeaderComponent', () => {
  let component: ShippingcusHeaderComponent;
  let fixture: ComponentFixture<ShippingcusHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingcusHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingcusHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
