import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberComponent } from './member.component';
import { ProfileComponent } from './profile/profile.component';
import { MmeberRoutingModule } from './member-routing.module';



@NgModule({
  declarations: [
    MemberComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    MmeberRoutingModule
  ],
  providers: [],
  exports: [MemberComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class MemberModule { }
