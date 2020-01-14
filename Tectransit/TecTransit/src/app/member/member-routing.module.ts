import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MemberComponent } from './member.component';
import { AuthGuard } from '../_Helper/auth.guard';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
  {
    path: 'member',
    component: MemberComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'profile',
        component: ProfileComponent,
      }
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class MmeberRoutingModule { }
