import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IfStmt } from '@angular/compiler';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;
  private baseUrl = window.location.origin + '/api/Login/';

  private loginUrl = 'doLogin';
  private logoutUrl = 'doLogout';

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }

  login(username, password) {
    const postData = { USERCODE: username, PASSWORD: password };
    return this.http.post<any>(this.baseUrl + this.loginUrl, postData)
      .pipe(map(user => {
        // store usercode in session storage to keep user logged in between page refreshes
        if (user.status === 'success') {
          sessionStorage.setItem('currentUser', user.id);
          this.currentUserSubject.next(user);
        }
        return user;
      }));
  }

  logout() {
    // remove serverside cookies
    this.http.get<any>(this.baseUrl + this.logoutUrl).subscribe();
    // remove user from session storage and set current user to null
    sessionStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
