import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegconfirmComponent } from './regconfirm.component';

describe('RegconfirmComponent', () => {
  let component: RegconfirmComponent;
  let fixture: ComponentFixture<RegconfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegconfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
