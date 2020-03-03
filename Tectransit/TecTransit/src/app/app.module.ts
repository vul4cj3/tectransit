import { BrowserModule } from '@angular/platform-browser';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MemberModule } from './member/member.module';
import { AuthenticationService } from './services/login.service';
import { AuthGuard } from './_Helper/auth.guard';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CommonService } from './services/common.service';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RegmemComponent } from './regmem/regmem.component';
import { RegcusComponent } from './regcus/regcus.component';
import { RegconfirmComponent } from './regconfirm/regconfirm.component';
import { RegfinalComponent } from './regfinal/regfinal.component';
import { ForgetpwComponent } from './forgetpw/forgetpw.component';
import { ShareModule } from './share/share.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BannerComponent } from './banner/banner.component';
import { NewsListComponent } from './news-list/news-list.component';
import { NewsDetailComponent } from './news-detail/news-detail.component';
import { FaqComponent } from './faq/faq.component';
import { ContactComponent } from './contact/contact.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavMenuComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    RegmemComponent,
    RegcusComponent,
    RegconfirmComponent,
    RegfinalComponent,
    ForgetpwComponent,
    BannerComponent,
    NewsListComponent,
    NewsDetailComponent,
    FaqComponent,
    ContactComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    MemberModule,
    AppRoutingModule,
    ShareModule,
    NgbModule
  ],
  schemas: [NO_ERRORS_SCHEMA],
  providers: [AuthenticationService, AuthGuard, CommonService],
  bootstrap: [AppComponent]
})
export class AppModule { }
