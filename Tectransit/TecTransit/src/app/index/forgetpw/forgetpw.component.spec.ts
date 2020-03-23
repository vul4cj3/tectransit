import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgetpwComponent } from './forgetpw.component';

describe('ForgetpwComponent', () => {
  let component: ForgetpwComponent;
  let fixture: ComponentFixture<ForgetpwComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForgetpwComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgetpwComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
