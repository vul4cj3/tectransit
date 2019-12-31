import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountsEditComponent } from './accounts-edit.component';

describe('AccountsEditComponent', () => {
  let component: AccountsEditComponent;
  let fixture: ComponentFixture<AccountsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountsEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
