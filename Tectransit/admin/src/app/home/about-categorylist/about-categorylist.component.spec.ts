import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutCategorylistComponent } from './about-categorylist.component';

describe('AboutCategorylistComponent', () => {
  let component: AboutCategorylistComponent;
  let fixture: ComponentFixture<AboutCategorylistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutCategorylistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutCategorylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
