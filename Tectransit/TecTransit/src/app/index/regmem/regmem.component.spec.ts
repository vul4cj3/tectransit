import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegmemComponent } from './regmem.component';

describe('RegmemComponent', () => {
  let component: RegmemComponent;
  let fixture: ComponentFixture<RegmemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegmemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegmemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
