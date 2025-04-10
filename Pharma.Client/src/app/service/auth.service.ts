import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { authCookieKey } from '../common/constant/auth-key';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  http: HttpClient;
  apiUrl: string = environment.apiUrl;


  constructor(private httpBacked: HttpBackend) {
    this.http = new HttpClient(this.httpBacked);
  }

  registerUser(user: any) {
    return this.http.post(this.apiUrl + '/v1/Auth/Register', user).pipe(map(user => {
      localStorage.setItem('pharmaKey', JSON.stringify(user));
      return user;
    }));
  }

  generateOtp(userId: string, otpType: string) {
    const URI = `${environment.apiUrl}/v1/otp/generate/${userId}/${otpType}`;
    return this.http.post(URI, { observe: 'response' });
  }
  login(user: any) {
    return this.http.post<any>(`${environment.apiUrl}/v1/Auth/Login`, user)
      .pipe(map(user => {
        if (user && user.accessToken) {
          localStorage.setItem('pharmaKey', JSON.stringify(user));
        }
        return user;
      }));
  }

  verifyOtp(model: any) {
    const URI = `${environment.apiUrl}/v1/otp/verify`;
    return this.http.post(URI, model, { observe: 'response' });
  }

  public getTokenInfo() {
    return localStorage.getItem(authCookieKey);
  }
  getRoles() {
    const tokenInfo = this.getTokenInfo();
    if (!tokenInfo) {
      return null;
    }
    const userData = JSON.parse(tokenInfo);
    const token = userData.accessToken;
    if (token == null) {
      return null;
    }
    const decodedToken = this.decodeToken(token);

    let roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    if (!Array.isArray(roles)) {
      roles = [roles];
    }

    if (roles.length > 1) {
      return roles[0];
    }

    return roles;
  }
  decodeToken(accessToken: any) {
    var base64Url = accessToken.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload) || {};
  }

  logOut() {
    localStorage.removeItem('pharmaKey');
  }
}
