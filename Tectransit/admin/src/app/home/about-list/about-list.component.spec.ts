import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutListComponent } from './about-list.component';

describe('AboutListComponent', () => {
  let component: AboutListComponent;
  let fixture: ComponentFixture<AboutListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
