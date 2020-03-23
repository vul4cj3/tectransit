import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CusindexComponent } from './cusindex.component';

describe('CusindexComponent', () => {
  let component: CusindexComponent;
  let fixture: ComponentFixture<CusindexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CusindexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CusindexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
