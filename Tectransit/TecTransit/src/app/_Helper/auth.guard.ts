import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '../services/login.service';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  private currentAcctSubject: BehaviorSubject<any>;
  public currentAcct: Observable<any>;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    this.currentAcctSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentAcct')));
    this.currentAcct = this.currentAcctSubject.asObservable();
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // check cookies expire or not
    if (document.cookie === '') {
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
