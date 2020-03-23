import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './index.component';
import { HomeComponent } from './home/home.component';
import { CustomListComponent } from './custom-list/custom-list.component';
import { CustomDetailComponent } from './custom-detail/custom-detail.component';
import { NewsListComponent } from './news-list/news-list.component';
import { NewsDetailComponent } from './news-detail/news-detail.component';
import { FaqComponent } from './faq/faq.component';
import { RegisterComponent } from './register/register.component';
import { ContactComponent } from './contact/contact.component';
import { RegmemComponent } from './regmem/regmem.component';
import { RegcusComponent } from './regcus/regcus.component';
import { RegconfirmComponent } from './regconfirm/regconfirm.component';
import { RegfinalComponent } from './regfinal/regfinal.component';
import { LoginComponent } from './login/login.component';
import { ForgetpwComponent } from './forgetpw/forgetpw.component';
import { SitemapComponent } from './sitemap/sitemap.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from '../_Helper/auth.guard';
import { StationComponent } from './station/station.component';
import { EntrustComponent } from './entrust/entrust.component';
import { ShippingListComponent } from './shipping-list/shipping-list.component';
import { ShippingEditComponent } from './shipping-edit/shipping-edit.component';
import { ShippingCombineComponent } from './shipping-combine/shipping-combine.component';
import { DeclarantComponent } from './declarant/declarant.component';

const routes: Routes = [
  {
    path: '',
    component: IndexComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'home' },
      { path: 'home', component: HomeComponent, },
      { path: 'custom/:id', component: CustomListComponent },
      { path: 'custom/detail/:cateid/:id', component: CustomDetailComponent },
      { path: 'news', component: NewsListComponent },
      { path: 'news/detail/:id', component: NewsDetailComponent },
      { path: 'faq', component: FaqComponent },
      { path: 'contact', component: ContactComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'regmem', component: RegmemComponent },
      { path: 'regcus', component: RegcusComponent },
      { path: 'regconfirm/:id', component: RegconfirmComponent },
      { path: 'regfinal', component: RegfinalComponent },
      { path: 'login', component: LoginComponent },
      { path: 'forgetpw', component: ForgetpwComponent },
      { path: 'sitemap', component: SitemapComponent },
    ],
  },
  {
    path: 'member',
    component: IndexComponent,
    children: [
      { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
      { path: 'station', component: StationComponent, canActivate: [AuthGuard] },
      { path: 'entrust', component: EntrustComponent, canActivate: [AuthGuard] },
      { path: 'shipping/:type/:id', component: ShippingListComponent, canActivate: [AuthGuard] },
      { path: 'shipping/edit/:type/:code/:id', component: ShippingEditComponent, canActivate: [AuthGuard] },
      { path: 'shipping/combine', component: ShippingCombineComponent, canActivate: [AuthGuard] },
      { path: 'declarant', component: DeclarantComponent, canActivate: [AuthGuard] }
    ],
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { enableTracing: false })
    // RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class IndexRoutingModule { }
