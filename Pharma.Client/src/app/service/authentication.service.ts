import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private registeredUser = new BehaviorSubject<any>(null);
  registeredUser$ = this.registeredUser.asObservable();
  private otpTimer = new BehaviorSubject<any>('');
  otpTimer$ = this.otpTimer.asObservable();
  apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  setUser(user: any) {
    this.registeredUser.next(user);
  }
  setOtpTimer(timer: any) {
    this.otpTimer.next(timer);
  }

  getPharmacyUser() {
    const URI = `${environment.apiUrl}/v1/Pharmacy/GetQRCode`;
    return this.http.get(URI, { observe: 'response' });
  }
}
