import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntrustcusImportComponent } from './entrustcus-import.component';

describe('EntrustcusImportComponent', () => {
  let component: EntrustcusImportComponent;
  let fixture: ComponentFixture<EntrustcusImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntrustcusImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntrustcusImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
