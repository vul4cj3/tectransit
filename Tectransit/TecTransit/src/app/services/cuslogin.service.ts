import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class CusloginService {
  private currentCusSubject: BehaviorSubject<any>;
  public currentCus: Observable<any>;
  private baseUrl = window.location.origin + '/api/Login/';

  private loginUrl = 'doCusLogin';
  private logoutUrl = 'doCusLogout';

  constructor(private http: HttpClient) {
    this.currentCusSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentCus')));
    this.currentCus = this.currentCusSubject.asObservable();
  }

  public get currentUserValue() {
    return this.currentCusSubject.value;
  }

  login(username, password, captcha) {
    const postData = { USERCODE: username, PASSWORD: password, CODE: captcha };
    return this.http.post<any>(this.baseUrl + this.loginUrl, postData)
      .pipe(map(user => {
        // store usercode in session storage to keep user logged in between page refreshes
        if (user.status === 'success') {
          sessionStorage.setItem('currentCus', user.id);
          this.currentCusSubject.next(user);
        }
        return user;
      }));
  }

  logout() {
    // remove serverside cookies
    this.http.get<any>(this.baseUrl + this.logoutUrl).subscribe();
    // remove user from session storage and set current user to null
    sessionStorage.removeItem('currentCus');
    this.currentCusSubject.next(null);
  }
}
