import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private registeredUser = new BehaviorSubject<any>(null);
  registeredUser$ = this.registeredUser.asObservable();
  private otpTimer = new BehaviorSubject<any>('');
  otpTimer$ = this.otpTimer.asObservable();

  constructor() { }

  setUser(user: any) {
    this.registeredUser.next(user);
  }
  setOtpTimer(timer: any) {
    this.otpTimer.next(timer);
  }
}
