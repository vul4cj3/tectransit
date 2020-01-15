import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegcusComponent } from './regcus.component';

describe('RegcusComponent', () => {
  let component: RegcusComponent;
  let fixture: ComponentFixture<RegcusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegcusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegcusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
