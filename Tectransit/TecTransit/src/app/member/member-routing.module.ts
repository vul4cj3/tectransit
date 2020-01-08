import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MemberComponent } from './member.component';
import { AuthGuard } from '../_Helper/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: MemberComponent,
    canActivate: [AuthGuard],
    children: [
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { enableTracing: true })
  ],
  exports: [RouterModule]
})
export class MmeberRoutingModule { }
