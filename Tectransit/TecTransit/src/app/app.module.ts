import { BrowserModule } from '@angular/platform-browser';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonService } from './services/common.service';
import { ShareModule } from './share/share.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { IndexModule } from './index/index.module';
import { IndexComponent } from './index/index.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { CusloginComponent } from './cuslogin/cuslogin.component';
import { CusindexComponent } from './cusindex/cusindex.component';
import { CusindexModule } from './cusindex/cusindex.module';
import { ShippstatusPipe } from './_Helper/shippstatus.pipe';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    NavMenuComponent,
    FooterComponent,
    CusloginComponent,
    CusindexComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    IndexModule,
    CusindexModule,
    AppRoutingModule,
    ShareModule,
    NgbModule
  ],
  schemas: [NO_ERRORS_SCHEMA],
  providers: [CommonService],
  bootstrap: [AppComponent]
})
export class AppModule { }
