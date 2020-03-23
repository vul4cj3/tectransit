import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { CusloginComponent } from './cuslogin/cuslogin.component';


const routes: Routes = [
  { path: 'cuslogin', component: CusloginComponent },
  // firsttime and otherwise redirect to HomeComponent
  { path: '**', component: IndexComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
