import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyrankListComponent } from './companyrank-list.component';

describe('CompanyrankListComponent', () => {
  let component: CompanyrankListComponent;
  let fixture: ComponentFixture<CompanyrankListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyrankListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyrankListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
