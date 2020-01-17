import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { AuthGuard } from '../_Helper/auth.guard';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleEditComponent } from './role-edit/role-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { MenuListComponent } from './menu-list/menu-list.component';
import { RankListComponent } from './rank-list/rank-list.component';
import { RankEditComponent } from './rank-edit/rank-edit.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountsEditComponent } from './accounts-edit/accounts-edit.component';
import { CompanyrankListComponent } from './companyrank-list/companyrank-list.component';
import { CompanyrankEditComponent } from './companyrank-edit/companyrank-edit.component';
import { CompanyListComponent } from './company-list/company-list.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { UserlogComponent } from './userlog/userlog.component';
import { BannerListComponent } from './banner-list/banner-list.component';
import { NewsListComponent } from './news-list/news-list.component';
import { NewsEditComponent } from './news-edit/news-edit.component';
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

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'roles',
        component: RoleListComponent,
      },
      {
        path: 'roles/edit/:id',
        component: RoleEditComponent,
      },
      {
        path: 'users',
        component: UserListComponent,
      },
      {
        path: 'users/edit/:id',
        component: UserEditComponent,
      },
      {
        path: 'userlog',
        component: UserlogComponent,
      },
      {
        path: 'menu',
        component: MenuListComponent,
      },
      {
        path: 'rank',
        component: RankListComponent,
      },
      {
        path: 'rank/edit/:id',
        component: RankEditComponent,
      },
      {
        path: 'accounts',
        component: AccountsListComponent,
      },
      {
        path: 'accounts/edit/:id',
        component: AccountsEditComponent,
      },
      {
        path: 'companyrank',
        component: CompanyrankListComponent,
      },
      {
        path: 'companyrank/edit/:id',
        component: CompanyrankEditComponent,
      },
      {
        path: 'company',
        component: CompanyListComponent,
      },
      {
        path: 'company/edit/:id',
        component: CompanyEditComponent,
      },
      {
        path: 'banner',
        component: BannerListComponent,
      },
      {
        path: 'news',
        component: NewsListComponent,
      },
      {
        path: 'news/edit/:id',
        component: NewsEditComponent,
      },
      {
        path: 'about',
        component: AboutCategorylistComponent,
      },
      {
        path: 'about/edit/:id',
        component: AboutCategoryeditComponent,
      },
      {
        path: 'about/info/:id',
        component: AboutListComponent,
      },
      {
        path: 'about/infoedit/:id/:id2',
        component: AboutEditComponent,
      },
      {
        path: 'faq',
        component: FaqCategorylistComponent,
      },
      {
        path: 'faq/edit/:id',
        component: FaqCategoryeditComponent,
      },
      {
        path: 'faq/info/:id',
        component: FaqListComponent,
      },
      {
        path: 'faq/infoedit/:id/:id2',
        component: FaqEditComponent,
      },
      {
        path: 'station',
        component: StationListComponent,
      },
      {
        path: 'station/edit/:id',
        component: StationEditComponent,
      },
      {
        path: 'transfer',
        component: TransferListComponent,
      },
      {
        path: 'transfer/edit/:id',
        component: TransferEditComponent,
      }
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { enableTracing: true })
  ],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
