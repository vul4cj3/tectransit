import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavMenucusComponent } from './nav-menucus.component';

describe('NavMenucusComponent', () => {
  let component: NavMenucusComponent;
  let fixture: ComponentFixture<NavMenucusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavMenucusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMenucusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
