import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyrankEditComponent } from './companyrank-edit.component';

describe('CompanyrankEditComponent', () => {
  let component: CompanyrankEditComponent;
  let fixture: ComponentFixture<CompanyrankEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyrankEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyrankEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
