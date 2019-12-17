import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeComponent } from './home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleEditComponent } from './role-edit/role-edit.component';
import { HomeRoutingModule } from './home-routing.module';

@NgModule({
  declarations: [
     HomeComponent,
     NavMenuComponent,
     RoleListComponent,
     RoleEditComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule
  ],
  exports: [HomeComponent]
})

export class HomeModule { }
