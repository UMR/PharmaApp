import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.production';
import { BehaviorSubject, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PharmacyMerchantService {
  http: HttpClient;
  apiUrl: string = environment.apiUrl;
  private userSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  public user: Observable<null> | undefined;


  constructor(private httpBacked: HttpBackend) {
    this.http = new HttpClient(this.httpBacked);
    this.userSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
    this.user = this.userSubject.asObservable();
  }

  registerUser(user: any) {
    return this.http.post(this.apiUrl + '/v1/Auth/Register', user).pipe(map(user => {
      localStorage.setItem('pharmaKey', JSON.stringify(user));
      this.userSubject.next(user);
      return user;
    }));
  }
  public get userValue() {
    return this.userSubject.value;
  }

  generateOtp(userId: string, otpType: string) {
    const URI = `${environment.apiUrl}/v1/otp/generate/${userId}/${otpType}`;
    return this.http.post(URI, { observe: 'response' });
  }
  login(user: any) {
    return this.http.post<any>(`${environment.apiUrl}/v1/Auth/Login`, user)
      .pipe(map(user => {
        localStorage.setItem('pharmaKey', JSON.stringify(user));
        this.userSubject.next(user);
        return user;
      }));
  }
  verifyOtp(model: any) {
    const URI = `${environment.apiUrl}/v1/otp/verify`;
    return this.http.post(URI, model, { observe: 'response' });
  }

  logOut() {
    localStorage.removeItem('pharmaKey');
    this.userSubject.next(null);
  }
}
