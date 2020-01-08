import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberComponent } from './member.component';



@NgModule({
  declarations: [
    MemberComponent
  ],
  imports: [
    CommonModule
  ],
  providers: [],
  exports: [MemberComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class MemberModule { }
