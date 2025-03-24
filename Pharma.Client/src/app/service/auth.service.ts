import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, map, Observable } from 'rxjs';

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
        localStorage.setItem('pharmaKey', JSON.stringify(user));
        return user;
      }));
  }
  verifyOtp(model: any) {
    const URI = `${environment.apiUrl}/v1/otp/verify`;
    return this.http.post(URI, model, { observe: 'response' });
  }

  logOut() {
    localStorage.removeItem('pharmaKey');
  }
}
