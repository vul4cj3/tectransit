import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CusloginComponent } from './cuslogin.component';

describe('CusloginComponent', () => {
  let component: CusloginComponent;
  let fixture: ComponentFixture<CusloginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CusloginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CusloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
