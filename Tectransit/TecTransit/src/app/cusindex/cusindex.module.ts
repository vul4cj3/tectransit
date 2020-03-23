import { NgModule } from '@angular/core';
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



@NgModule({
  declarations: [
    EntrustcusComponent,
    ProfilecusComponent,
    ShippingcusListComponent,
    ShippingcusEditComponent,
    ShippingcusHeaderComponent,
    EntrustcusImportComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    CusIndexRoutingModule,
    ShareModule
  ]
})
export class CusindexModule { }
