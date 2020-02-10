import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingcusEditComponent } from './shippingcus-edit.component';

describe('ShippingcusEditComponent', () => {
  let component: ShippingcusEditComponent;
  let fixture: ComponentFixture<ShippingcusEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingcusEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingcusEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
