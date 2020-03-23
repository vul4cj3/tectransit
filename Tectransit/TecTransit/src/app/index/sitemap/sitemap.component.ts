import { Component, OnInit } from '@angular/core';
import { MenuInfo, AboutCate } from '../../_Helper/models';
import { CommonService } from '../../services/common.service';
import { AuthenticationService } from '../../services/login.service';

@Component({
  selector: 'app-sitemap',
  templateUrl: './sitemap.component.html',
  styleUrls: ['./sitemap.component.css']
})
export class SitemapComponent implements OnInit {

  ismemLogin = false;
  ismemcus = false;

  data: MenuInfo[];
  subdata: MenuInfo[];
  aboutdata: AboutCate[];

  constructor(
    private authenticationService: AuthenticationService,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.chkMem();
    this.getMenu();
  }

  chkMem() {
    const currentAcct = sessionStorage.getItem('currentAcct');
    if (currentAcct) {
      this.ismemLogin = true;
    }

    this.commonservice.chkMemtype()
      .subscribe(data => {
        if (data.data === 'Y') {
          this.ismemcus = true;
        } else { this.ismemcus = false; }
      },
        error => {
          console.log(error);
        });
  }

  getMenu() {
    this.commonservice.getMenu()
      .subscribe(data => {
        if (data.status === '0') {
          this.data = data.pList;
          this.subdata = data.item;
          this.aboutdata = data.aboutitem;
        }
      }, error => {
        console.log(error);
      });
  }

  doLogout() {
    this.authenticationService.logout();
    // redirect to login page
    document.location.href = '/';
  }

}
