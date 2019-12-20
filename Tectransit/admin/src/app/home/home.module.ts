import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeComponent } from './home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleEditComponent } from './role-edit/role-edit.component';
import { HomeRoutingModule } from './home-routing.module';
import { CommonService } from '../services/common.service';
import { PaginationComponent } from './pagination/pagination.component';
import { SysService } from '../services/sys.service';

@NgModule({
  declarations: [
     HomeComponent,
     NavMenuComponent,
     RoleListComponent,
     RoleEditComponent,
     PaginationComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule
  ],
  providers: [CommonService, SysService],
  exports: [HomeComponent]
})

export class HomeModule { }
