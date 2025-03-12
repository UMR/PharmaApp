import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private registeredUser = new BehaviorSubject<any>(null);
  registeredUser$ = this.registeredUser.asObservable();
  private otpTimer = new BehaviorSubject<number>(0);
  otpTimer$ = this.otpTimer.asObservable();

  constructor() { }

  setUser(user: any) {
    this.registeredUser.next(user);
  }
  setOtpTimer(timer: number) {
    this.otpTimer.next(timer);
  }
}
