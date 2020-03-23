import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilecusComponent } from './profilecus.component';

describe('ProfilecusComponent', () => {
  let component: ProfilecusComponent;
  let fixture: ComponentFixture<ProfilecusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfilecusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfilecusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
