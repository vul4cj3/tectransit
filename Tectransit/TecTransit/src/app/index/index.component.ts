import { Component, OnInit, Inject, HostListener } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { AuthenticationService } from '../services/login.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  userActivity;
  isScroll = false;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private authenticationService: AuthenticationService
  ) {
  }

  ngOnInit() {
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    if (document.body.scrollTop > 0 || document.documentElement.scrollTop > 0) {
      this.isScroll = true;
    } else if (document.body.scrollTop === 0 || document.documentElement.scrollTop === 0) {
      this.isScroll = false;
    }
  }

  @HostListener('window:mousemove', [])
  onWindowMousemove() {
    clearTimeout(this.userActivity);
    this.setLoginTimeout();
  }

  clearnav() {
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < document.getElementsByClassName('subnav').length; i++) {
      const item = document.getElementsByClassName('subnav')[i];
      item.classList.remove('active');
    }

    const profileitem = document.getElementsByClassName('open')[0];
    if (profileitem !== undefined) {
      profileitem.classList.remove('open');
    }
  }

  gotoTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth'
    });
  }

  setLoginTimeout() {
    // user閒置1hr則自動登出
    this.userActivity = setTimeout(() => {
      const currentAcct = sessionStorage.getItem('currentAcct');
      if (currentAcct) {
        this.authenticationService.logout();
        document.location.href = '/login';
      }
    }, 3600000);
  }

}
