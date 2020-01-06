import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutCategoryeditComponent } from './about-categoryedit.component';

describe('AboutCategoryeditComponent', () => {
  let component: AboutCategoryeditComponent;
  let fixture: ComponentFixture<AboutCategoryeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutCategoryeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutCategoryeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
