import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntrustEditComponent } from './entrust-edit.component';

describe('EntrustEditComponent', () => {
  let component: EntrustEditComponent;
  let fixture: ComponentFixture<EntrustEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntrustEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntrustEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
