import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IndexRoutingModule } from './index-routing.module';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RegmemComponent } from './regmem/regmem.component';
import { RegcusComponent } from './regcus/regcus.component';
import { RegconfirmComponent } from './regconfirm/regconfirm.component';
import { RegfinalComponent } from './regfinal/regfinal.component';
import { ForgetpwComponent } from './forgetpw/forgetpw.component';
import { BannerComponent } from './banner/banner.component';
import { NewsListComponent } from './news-list/news-list.component';
import { NewsDetailComponent } from './news-detail/news-detail.component';
import { FaqComponent } from './faq/faq.component';
import { ContactComponent } from './contact/contact.component';
import { SitemapComponent } from './sitemap/sitemap.component';
import { CustomListComponent } from './custom-list/custom-list.component';
import { CustomDetailComponent } from './custom-detail/custom-detail.component';
import { SafeHtmlPipe } from '../_Helper/safeHtml.pipe';
import { AuthenticationService } from '../services/login.service';
import { AuthGuard } from '../_Helper/auth.guard';
import { CommonService } from '../services/common.service';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ShareModule } from '../share/share.module';
import { ProfileComponent } from './profile/profile.component';
import { StationComponent } from './station/station.component';
import { EntrustComponent } from './entrust/entrust.component';
import { ShippingHeaderComponent } from './shipping-header/shipping-header.component';
import { DeclarantComponent } from './declarant/declarant.component';
import { ShippingCombineComponent } from './shipping-combine/shipping-combine.component';
import { ShippingEditComponent } from './shipping-edit/shipping-edit.component';
import { ShippingListComponent } from './shipping-list/shipping-list.component';



@NgModule({
  declarations: [
    HomeComponent,
    BannerComponent,
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
    ContactComponent,
    CustomListComponent,
    CustomDetailComponent,
    SafeHtmlPipe,
    SitemapComponent,
    ProfileComponent,
    StationComponent,
    EntrustComponent,
    ShippingListComponent,
    ShippingEditComponent,
    ShippingCombineComponent,
    DeclarantComponent,
    ShippingHeaderComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    IndexRoutingModule,
    ShareModule,
    NgbModule
  ],
  exports: [],
  providers: [AuthenticationService, AuthGuard, CommonService],
  schemas: [NO_ERRORS_SCHEMA]
})
export class IndexModule { }
