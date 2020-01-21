import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MemberComponent } from './member.component';
import { AuthGuard } from '../_Helper/auth.guard';
import { ProfileComponent } from './profile/profile.component';
import { StationComponent } from './station/station.component';
import { EntrustComponent } from './entrust/entrust.component';
import { EntrustEditComponent } from './entrust-edit/entrust-edit.component';
import { ShippingListComponent } from './shipping-list/shipping-list.component';
import { ShippingEditComponent } from './shipping-edit/shipping-edit.component';
import { ShippingCombineComponent } from './shipping-combine/shipping-combine.component';

const routes: Routes = [
  {
    path: 'member',
    component: MemberComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'profile',
        component: ProfileComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'station',
        component: StationComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'entrust',
        component: EntrustComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'entrust/edit/:id',
        component: EntrustEditComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'shipping/edit/:type/:code/:id',
        component: ShippingEditComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'shipping/:type/:id',
        component: ShippingListComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'shipping/combine',
        component: ShippingCombineComponent,
        canActivate: [AuthGuard],
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
