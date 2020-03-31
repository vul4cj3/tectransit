import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExbrokerListComponent } from './exbroker-list.component';

describe('ExbrokerListComponent', () => {
  let component: ExbrokerListComponent;
  let fixture: ComponentFixture<ExbrokerListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExbrokerListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExbrokerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
