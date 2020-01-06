import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FaqCategorylistComponent } from './faq-categorylist.component';

describe('FaqCategorylistComponent', () => {
  let component: FaqCategorylistComponent;
  let fixture: ComponentFixture<FaqCategorylistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FaqCategorylistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FaqCategorylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
