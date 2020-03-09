import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberComponent } from './member.component';
import { ProfileComponent } from './profile/profile.component';
import { MmeberRoutingModule } from './member-routing.module';
import { StationComponent } from './station/station.component';
import { EntrustComponent } from './entrust/entrust.component';
import { ShippingListComponent } from './shipping-list/shipping-list.component';
import { ShippingEditComponent } from './shipping-edit/shipping-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ShippingCombineComponent } from './shipping-combine/shipping-combine.component';
import { DeclarantComponent } from './declarant/declarant.component';
import { EntrustcusComponent } from './entrustcus/entrustcus.component';
import { ProfilecusComponent } from './profilecus/profilecus.component';
import { ShippingcusListComponent } from './shippingcus-list/shippingcus-list.component';
import { ShippingcusEditComponent } from './shippingcus-edit/shippingcus-edit.component';
import { ShippingcusHeaderComponent } from './shippingcus-header/shippingcus-header.component';
import { ShareModule } from '../share/share.module';
import { ShippingHeaderComponent } from './shipping-header/shipping-header.component';
import { ShippstatusPipe } from '../_Helper/shippstatus.pipe';
import { EntrustcusImportComponent } from './entrustcus-import/entrustcus-import.component';



@NgModule({
  declarations: [
    MemberComponent,
    ProfileComponent,
    StationComponent,
    EntrustComponent,
    ShippingListComponent,
    ShippingEditComponent,
    ShippingCombineComponent,
    DeclarantComponent,
    EntrustcusComponent,
    ProfilecusComponent,
    ShippingcusListComponent,
    ShippingcusEditComponent,
    ShippingcusHeaderComponent,
    ShippingHeaderComponent,
    ShippstatusPipe,
    EntrustcusImportComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MmeberRoutingModule,
    ShareModule
  ],
  providers: [],
  exports: [MemberComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class MemberModule { }
