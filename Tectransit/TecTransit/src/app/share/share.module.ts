import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from './pagination/pagination.component';
import { ShippstatusPipe } from '../_Helper/shippstatus.pipe';



@NgModule({
  declarations: [
    PaginationComponent,
    ShippstatusPipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PaginationComponent,
    ShippstatusPipe
  ]
})
export class ShareModule { }
