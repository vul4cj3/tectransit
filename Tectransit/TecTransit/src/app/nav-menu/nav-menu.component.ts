import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/login.service';
import { Router } from '@angular/router';
import { CommonService } from '../services/common.service';
import { MenuInfo } from '../_Helper/models';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  ismemLogin = false;
  ismemcus = false;
  isactive = false;
  isScroll = false;

  data: MenuInfo[];
  subdata: MenuInfo[];

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
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

  chgSubnav(e) {
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < document.getElementsByClassName('active').length; i++) {
      const item = document.getElementsByClassName('active')[i];
      item.classList.remove('active');
    }

    const control = e.target.nextSibling as HTMLElement;
    if (control !== null) {
      control.classList.add('active');
    }
  }

  chgProfilelist(e) {
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < document.getElementsByClassName('open').length; i++) {
      const item = document.getElementsByClassName('open')[i];
      item.classList.remove('open');
    }

    const control = document.getElementsByClassName('profilelist')[0];
    if (control !== null) {
      control.classList.add('open');
    }
  }

}
