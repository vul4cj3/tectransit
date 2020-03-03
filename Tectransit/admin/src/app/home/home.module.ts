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
import { NewsListComponent } from './news-list/news-list.component';
import { NewsEditComponent } from './news-edit/news-edit.component';
import { CKEditorModule } from 'ckeditor4-angular';
import { AboutCategorylistComponent } from './about-categorylist/about-categorylist.component';
import { AboutCategoryeditComponent } from './about-categoryedit/about-categoryedit.component';
import { AboutListComponent } from './about-list/about-list.component';
import { AboutEditComponent } from './about-edit/about-edit.component';
import { FaqCategorylistComponent } from './faq-categorylist/faq-categorylist.component';
import { FaqCategoryeditComponent } from './faq-categoryedit/faq-categoryedit.component';
import { FaqListComponent } from './faq-list/faq-list.component';
import { FaqEditComponent } from './faq-edit/faq-edit.component';
import { StationListComponent } from './station-list/station-list.component';
import { StationEditComponent } from './station-edit/station-edit.component';
import { TransferListComponent } from './transfer-list/transfer-list.component';
import { TransferEditComponent } from './transfer-edit/transfer-edit.component';
import { ShippingcusListComponent } from './shippingcus-list/shippingcus-list.component';
import { ShippingcusEditComponent } from './shippingcus-edit/shippingcus-edit.component';
import { ShippstatusPipe } from '../_Helper/shippstatus.pipe';

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
    BannerListComponent,
    NewsListComponent,
    NewsEditComponent,
    AboutCategorylistComponent,
    AboutCategoryeditComponent,
    AboutListComponent,
    AboutEditComponent,
    FaqCategorylistComponent,
    FaqCategoryeditComponent,
    FaqListComponent,
    FaqEditComponent,
    StationListComponent,
    StationEditComponent,
    TransferListComponent,
    TransferEditComponent,
    ShippingcusListComponent,
    ShippingcusEditComponent,
    ShippstatusPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HomeRoutingModule,
    CKEditorModule
  ],
  providers: [CommonService, ModalService, ConfirmService],
  exports: [HomeComponent],
  schemas: [NO_ERRORS_SCHEMA]
})

export class HomeModule { }
