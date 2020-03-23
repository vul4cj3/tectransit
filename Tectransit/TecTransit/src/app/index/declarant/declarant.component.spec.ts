import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeclarantComponent } from './declarant.component';

describe('DeclarantComponent', () => {
  let component: DeclarantComponent;
  let fixture: ComponentFixture<DeclarantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeclarantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeclarantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
