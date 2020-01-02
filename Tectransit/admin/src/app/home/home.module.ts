import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeComponent } from './home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleEditComponent } from './role-edit/role-edit.component';
import { HomeRoutingModule } from './home-routing.module';
import { CommonService } from '../services/common.service';
import { PaginationComponent } from './pagination/pagination.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { UserListComponent } from './user-list/user-list.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { ModalService } from '../services/modal.service';
import { ModalComponent } from './modal.component';
import { ConfirmService } from '../services/confirm.service';
import { ConfirmComponent } from './confirm.component';
import { MenuListComponent } from './menu-list/menu-list.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountsEditComponent } from './accounts-edit/accounts-edit.component';
import { RankListComponent } from './rank-list/rank-list.component';
import { RankEditComponent } from './rank-edit/rank-edit.component';
import { CompanyrankListComponent } from './companyrank-list/companyrank-list.component';
import { CompanyrankEditComponent } from './companyrank-edit/companyrank-edit.component';
import { CompanyListComponent } from './company-list/company-list.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { UserlogComponent } from './userlog/userlog.component';
import { BannerListComponent } from './banner-list/banner-list.component';

@NgModule({
  declarations: [
    HomeComponent,
    NavMenuComponent,
    RoleListComponent,
    RoleEditComponent,
    PaginationComponent,
    UserListComponent,
    UserEditComponent,
    ModalComponent,
    ConfirmComponent,
    MenuListComponent,
    AccountsListComponent,
    AccountsEditComponent,
    RankListComponent,
    RankEditComponent,
    CompanyrankListComponent,
    CompanyrankEditComponent,
    CompanyListComponent,
    CompanyEditComponent,
    UserlogComponent,
    BannerListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HomeRoutingModule
  ],
  providers: [CommonService, ModalService, ConfirmService],
  exports: [HomeComponent]
})

export class HomeModule { }
