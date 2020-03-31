import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { EntrustcusComponent } from './entrustcus/entrustcus.component';
import { ProfilecusComponent } from './profilecus/profilecus.component';
import { ShippingcusListComponent } from './shippingcus-list/shippingcus-list.component';
import { ShippingcusEditComponent } from './shippingcus-edit/shippingcus-edit.component';
import { ShippingcusHeaderComponent } from './shippingcus-header/shippingcus-header.component';
import { EntrustcusImportComponent } from './entrustcus-import/entrustcus-import.component';
import { CusIndexRoutingModule } from './cusindex-routing.module';
import { ShareModule } from '../share/share.module';
import { NavMenucusComponent } from './nav-menucus/nav-menucus.component';
import { CusindexComponent } from './cusindex.component';
import { CommonService } from '../services/common.service';
import { ImbrokerListComponent } from './imbroker-list/imbroker-list.component';
import { ExbrokerListComponent } from './exbroker-list/exbroker-list.component';



@NgModule({
  declarations: [
    CusindexComponent,
    EntrustcusComponent,
    ProfilecusComponent,
    ShippingcusListComponent,
    ShippingcusEditComponent,
    ShippingcusHeaderComponent,
    EntrustcusImportComponent,
    NavMenucusComponent,
    ImbrokerListComponent,
    ExbrokerListComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    CusIndexRoutingModule,
    ShareModule
  ],
  providers: [CommonService],
  exports: [CusindexComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class CusindexModule { }
