import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CusindexComponent } from './cusindex.component';
import { ProfilecusComponent } from './profilecus/profilecus.component';
import { EntrustcusComponent } from './entrustcus/entrustcus.component';
import { EntrustcusImportComponent } from './entrustcus-import/entrustcus-import.component';
import { ShippingcusListComponent } from './shippingcus-list/shippingcus-list.component';
import { ShippingcusEditComponent } from './shippingcus-edit/shippingcus-edit.component';
import { CusauthGuard } from '../_Helper/cusauth.guard';

const routes: Routes = [
  {
    path: 'cus',
    component: CusindexComponent,
    canActivate: [CusauthGuard],
    children: [
      { path: 'profile', component: ProfilecusComponent, canActivate: [CusauthGuard] },
      { path: 'entrust', component: EntrustcusComponent, canActivate: [CusauthGuard] },
      { path: 'entrustim', component: EntrustcusImportComponent, canActivate: [CusauthGuard] },
      { path: 'shipping/:type', component: ShippingcusListComponent, canActivate: [CusauthGuard] },
      { path: 'shipping/edit/:type/:id', component: ShippingcusEditComponent, canActivate: [CusauthGuard] }
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { enableTracing: false })
    // RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class CusIndexRoutingModule { }
