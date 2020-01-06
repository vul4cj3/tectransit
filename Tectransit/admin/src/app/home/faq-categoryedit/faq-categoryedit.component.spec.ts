import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FaqCategoryeditComponent } from './faq-categoryedit.component';

describe('FaqCategoryeditComponent', () => {
  let component: FaqCategoryeditComponent;
  let fixture: ComponentFixture<FaqCategoryeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FaqCategoryeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FaqCategoryeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
