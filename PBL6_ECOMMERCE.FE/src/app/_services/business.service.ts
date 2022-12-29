
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/app-user';

const TOKEN_KEY = 'token'
const API =  'https://localhost:7220';
const LOGIN_URL = API + '/api/Auth/login';
const REGISTER_URL = API + '/api/Auth/register';

@Injectable({
  providedIn: 'root'
})
export class BusinessService {

  constructor(
    private httpClient: HttpClient
  ) { }

  login(data:any){
    console.log(data);
    return this.httpClient.post(LOGIN_URL, JSON.stringify(data), this.getRequestOptions())
  }
  register(data:any){
    return this.httpClient.post(REGISTER_URL, JSON.stringify(data))
  }

  setToken(token:any){
    localStorage.setItem(TOKEN_KEY, token)
  }

  getToken(){
    return localStorage.getItem(TOKEN_KEY)
  }

  getRequestOptions(){
    const token = this.getToken()
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      })
    }
    return httpOptions
  }


}
