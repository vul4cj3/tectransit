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

const routes: Routes = [
  {
    path: 'member',
    component: MemberComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'profile',
        component: ProfileComponent,
      },
      {
        path: 'station',
        component: StationComponent,
      },
      {
        path: 'entrust',
        component: EntrustComponent,
      },
      {
        path: 'entrust/edit/:id',
        component: EntrustEditComponent,
      },
      {
        path: 'shipping',
        component: ShippingListComponent,
      },
      {
        path: 'shipping/edit/:id',
        component: ShippingEditComponent,
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
