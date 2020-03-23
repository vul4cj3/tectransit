import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingcusListComponent } from './shippingcus-list.component';

describe('ShippingcusListComponent', () => {
  let component: ShippingcusListComponent;
  let fixture: ComponentFixture<ShippingcusListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingcusListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingcusListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
