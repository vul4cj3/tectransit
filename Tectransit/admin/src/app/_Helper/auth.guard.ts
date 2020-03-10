import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '../services/login.service';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  isExpire = 0;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // check cookies expire or not
    this.isExpire = 0;
    if (document.cookie !== '') {
      document.cookie.split(';').filter(item => {
        if (item.trim().indexOf('_usercode=') === 0) {
          this.isExpire++;
        } else if (item.trim().indexOf('_username=') === 0) {
          this.isExpire++;
        } else { }
      });
    } else { this.isExpire = 0; }

    if (this.isExpire < 2) {
      sessionStorage.removeItem('currentUser'); // logout
      this.currentUserSubject.next(null);
    }

    // const currentUser = this.authenticationService.currentUserValue;
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      // authorised so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
