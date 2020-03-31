import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImbrokerListComponent } from './imbroker-list.component';

describe('ImbrokerListComponent', () => {
  let component: ImbrokerListComponent;
  let fixture: ComponentFixture<ImbrokerListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImbrokerListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImbrokerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
