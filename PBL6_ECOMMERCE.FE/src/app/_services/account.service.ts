import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { User } from 'src/app/_models/app-user';
import { UserToken } from 'src/app/_models/app-user';
import { RegisterUser } from 'src/app/_models/app-user';

@Injectable({
  providedIn: 'root'
})
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  baseUrl: string = 'https://localhost:7220/api/Auth/';
  private currentUser = new BehaviorSubject<UserToken | null>(null);
  currentUser$ = this.currentUser.asObservable();

  constructor(private httpClient: HttpClient) {}

  login(user: User): Observable<any> {
    console.log(user.username);
    return this.httpClient
      .post(`${this.baseUrl}login`, user, {
        headers: this.headers,
        responseType: 'text',
      })
      .pipe(
        map((token) => {
          if (token) {
            const userToken: UserToken = {
              username: user.username,
              token,
            };
            this.currentUser.next(userToken);
            localStorage.setItem('userToken', JSON.stringify(userToken));
            // localStorage.setItem('token', 'JWT');
          }
        })
      );
  }
  logout() {
    this.currentUser.next(null);
    localStorage.removeItem('token');
  }

  register(registerUser: RegisterUser): Observable<any> {
    return this.httpClient
      .post(`${this.baseUrl}register`, registerUser, {
        headers: this.headers,
        responseType: 'text',
      })
      .pipe(
        map((token) => {
          if (token) {
            const userToken: UserToken = {
              username: registerUser.username,
              token,
            };
            this.currentUser.next(userToken);
            localStorage.setItem('userToken', JSON.stringify(userToken));
          }
        })
      );
      }
}
