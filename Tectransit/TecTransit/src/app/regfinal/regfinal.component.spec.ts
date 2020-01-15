import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegfinalComponent } from './regfinal.component';

describe('RegfinalComponent', () => {
  let component: RegfinalComponent;
  let fixture: ComponentFixture<RegfinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegfinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegfinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
