import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntrustcusComponent } from './entrustcus.component';

describe('EntrustcusComponent', () => {
  let component: EntrustcusComponent;
  let fixture: ComponentFixture<EntrustcusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntrustcusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntrustcusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
