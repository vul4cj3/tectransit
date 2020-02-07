import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/login.service';
import { Router } from '@angular/router';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  ismemLogin = false;
  ismemcus = false;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.chkMem();
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

  doLogout() {
    this.authenticationService.logout();
    // redirect to login page
    document.location.href = '/';
  }

}
