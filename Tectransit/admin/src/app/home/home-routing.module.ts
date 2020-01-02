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
