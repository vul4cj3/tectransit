import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentAcctSubject: BehaviorSubject<any>;
  public currentAcct: Observable<any>;
  private baseUrl = window.location.origin + '/api/Login/';

  private loginUrl = 'doAccLogin';
  private logoutUrl = 'doAccLogout';

  constructor(private http: HttpClient) {
    this.currentAcctSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentAcct')));
    this.currentAcct = this.currentAcctSubject.asObservable();
  }

  public get currentUserValue() {
    return this.currentAcctSubject.value;
  }

  login(username, password) {
    const postData = { USERCODE: username, PASSWORD: password };
    return this.http.post<any>(this.baseUrl + this.loginUrl, postData)
      .pipe(map(user => {
        // store usercode in session storage to keep user logged in between page refreshes
        if (user.status === 'success') {
          sessionStorage.setItem('currentAcct', user.id);
          this.currentAcctSubject.next(user);
        }
        return user;
      }));
  }

  logout() {
    // remove serverside cookies
    this.http.get<any>(this.baseUrl + this.logoutUrl).subscribe();
    // remove user from session storage and set current user to null
    sessionStorage.removeItem('currentAcct');
    this.currentAcctSubject.next(null);
  }
}
