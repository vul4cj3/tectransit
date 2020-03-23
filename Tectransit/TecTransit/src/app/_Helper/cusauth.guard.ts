import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { CusloginService } from '../services/cuslogin.service';

@Injectable({ providedIn: 'root' })
export class CusauthGuard implements CanActivate {
  private currentCusSubject: BehaviorSubject<any>;
  public currentCus: Observable<any>;

  isExpire = 0;

  constructor(
    private router: Router,
    private cusloginservice: CusloginService
  ) {
    this.currentCusSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentCus')));
    this.currentCus = this.currentCusSubject.asObservable();
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // check cookies expire or not
    this.isExpire = 0;
    if (document.cookie !== '') {
      document.cookie.split(';').filter(item => {
        if (item.trim().indexOf('_cuscode=') === 0) {
          this.isExpire++;
        } else if (item.trim().indexOf('_cusname=') === 0) {
          this.isExpire++;
        } else { }
      });
    } else { this.isExpire = 0; }

    if (this.isExpire < 2) {
      sessionStorage.removeItem('currentCus'); // logout
      this.currentCusSubject.next(null);
    }

    // const currentAcct = this.authenticationService.currentAcctValue;
    const currentCus = sessionStorage.getItem('currentCus');
    if (currentCus) {
      // authorised so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    document.location.href = '/cuslogin';
    return false;
  }

}
