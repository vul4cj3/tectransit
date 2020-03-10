import { Injectable, HostListener } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '../services/login.service';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  private currentAcctSubject: BehaviorSubject<any>;
  public currentAcct: Observable<any>;

  isExpire = 0;
  userActivity;
  userInactive: Subject<any> = new Subject();


  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    this.currentAcctSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentAcct')));
    this.currentAcct = this.currentAcctSubject.asObservable();
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // check cookies expire or not
    this.isExpire = 0;
    if (document.cookie !== '') {
      document.cookie.split(';').filter(item => {
        if (item.trim().indexOf('_acccode=') === 0) {
          this.isExpire++;
        } else if (item.trim().indexOf('_accname=') === 0) {
          this.isExpire++;
        } else { }
      });
    } else { this.isExpire = 0; }

    if (this.isExpire < 2) {
      sessionStorage.removeItem('currentAcct'); // logout
      this.currentAcctSubject.next(null);
    }

    // const currentAcct = this.authenticationService.currentAcctValue;
    const currentAcct = sessionStorage.getItem('currentAcct');
    if (currentAcct) {
      // authorised so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    document.location.href = '/login';
    return false;
  }

}
