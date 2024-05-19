import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  readonly apiUrl = "http://localhost:53535/api/";
  readonly photoUrl = "http://localhost:53535/Photos/";
  private userDataSubject = new BehaviorSubject<any>(null);
  userInfo$ = this.userDataSubject.asObservable();
  constructor(private http: HttpClient) { }
  setUserData(userInfo: any) {
    this.userDataSubject.next(userInfo)
  }

  getUserData() {
    return this.userDataSubject.value;
  }
  setLoggedIn(value: boolean) {
    this.isLoggedInSubject.next(value);

  }
  LoginSystem(userId:string,password:string): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'Login/Login/'+userId+'/'+password);
   }
   clearUserData() {
    this.userDataSubject.next(null); // Reset user data to null
    
  }
}