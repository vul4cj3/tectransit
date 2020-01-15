import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  ismemLogin = false;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) { }

  ngOnInit() {
    const currentAcct = sessionStorage.getItem('currentAcct');
    if (currentAcct) {
      this.ismemLogin = true;
    }

  }

  doLogout() {
    this.authenticationService.logout();
    // redirect to login page
    document.location.href = '/';
  }

}
