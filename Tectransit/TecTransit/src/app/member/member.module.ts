import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberComponent } from './member.component';
import { ProfileComponent } from './profile/profile.component';
import { MmeberRoutingModule } from './member-routing.module';
import { StationComponent } from './station/station.component';
import { EntrustComponent } from './entrust/entrust.component';
import { ShippingListComponent } from './shipping-list/shipping-list.component';
import { ShippingEditComponent } from './shipping-edit/shipping-edit.component';
import { EntrustEditComponent } from './entrust-edit/entrust-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ShippingCombineComponent } from './shipping-combine/shipping-combine.component';



@NgModule({
  declarations: [
    MemberComponent,
    ProfileComponent,
    StationComponent,
    EntrustComponent,
    ShippingListComponent,
    ShippingEditComponent,
    EntrustEditComponent,
    ShippingCombineComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MmeberRoutingModule
  ],
  providers: [],
  exports: [MemberComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class MemberModule { }
