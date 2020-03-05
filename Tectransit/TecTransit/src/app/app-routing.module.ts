import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { RegmemComponent } from './regmem/regmem.component';
import { RegcusComponent } from './regcus/regcus.component';
import { RegconfirmComponent } from './regconfirm/regconfirm.component';
import { RegfinalComponent } from './regfinal/regfinal.component';
import { ForgetpwComponent } from './forgetpw/forgetpw.component';
import { NewsListComponent } from './news-list/news-list.component';
import { NewsDetailComponent } from './news-detail/news-detail.component';
import { FaqComponent } from './faq/faq.component';
import { ContactComponent } from './contact/contact.component';
import { CustomListComponent } from './custom-list/custom-list.component';
import { CustomDetailComponent } from './custom-detail/custom-detail.component';
import { SitemapComponent } from './sitemap/sitemap.component';


const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'custom/:id', component: CustomListComponent},
  {path: 'custom/detail/:cateid/:id', component: CustomDetailComponent},
  {path: 'news', component: NewsListComponent},
  {path: 'news/detail/:id', component: NewsDetailComponent},
  {path: 'faq', component: FaqComponent},
  {path: 'contact', component: ContactComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'regmem', component: RegmemComponent},
  {path: 'regcus', component: RegcusComponent},
  {path: 'regconfirm/:id', component: RegconfirmComponent},
  {path: 'regfinal', component: RegfinalComponent},
  {path: 'login', component: LoginComponent},
  {path: 'forgetpw', component: ForgetpwComponent},
  {path: 'sitemap', component: SitemapComponent},

  // firsttime and otherwise redirect to HomeComponent
  {path: '**', component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
