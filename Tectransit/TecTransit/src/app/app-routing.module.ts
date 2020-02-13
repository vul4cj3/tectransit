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


const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'about', component: HomeComponent},
  {path: 'news', component: HomeComponent},
  {path: 'contact', component: HomeComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'regmem', component: RegmemComponent},
  {path: 'regcus', component: RegcusComponent},
  {path: 'regconfirm/:id', component: RegconfirmComponent},
  {path: 'regfinal', component: RegfinalComponent},
  {path: 'login', component: LoginComponent},
  {path: 'forgetpw', component: ForgetpwComponent},

  // firsttime and otherwise redirect to HomeComponent
  {path: '**', component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
