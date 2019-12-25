import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { AuthGuard } from '../_Helper/auth.guard';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleEditComponent } from './role-edit/role-edit.component';

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
      }
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {enableTracing: true })
  ],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
