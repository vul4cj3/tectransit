import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CusloginComponent } from './cuslogin/cuslogin.component';
import { MainComponent } from './main/main.component';


const routes: Routes = [
  { path: 'cuslogin', component: CusloginComponent },
  // firsttime and otherwise redirect to HomeComponent
  { path: '**', component: MainComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
