import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RankEditComponent } from './rank-edit.component';

describe('RankEditComponent', () => {
  let component: RankEditComponent;
  let fixture: ComponentFixture<RankEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RankEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RankEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
