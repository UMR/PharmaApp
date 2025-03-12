import { Component, HostListener, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../service/authentication.service';
import { PharmacyMerchantService } from '../../../service/pharmacy-merchant.service';
import { first, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { ToastMessageService } from '../../../service/toast-message.service';

@Component({
  selector: 'app-otp',
  standalone: false,
  templateUrl: './otp.component.html',
  styleUrl: './otp.component.css'
})
export class OtpComponent implements OnInit {
  /**
   *
   */
  constructor(private authService: AuthenticationService, private pharmacyMerchantService: PharmacyMerchantService, private router: Router, private toastService: ToastMessageService) {

  }
  _otpTimer: Subscription | any;
  otpTimer: any;;
  showResendBtn: boolean = false;
  user: any;
  otp: any;

  ngOnInit(): void {
    this._otpTimer = this.authService.otpTimer$.subscribe((timer) => this.otpTimer = this.getTimer(60));
  }


  getTimer(timer: number) {
    this.showResendBtn = false;
    let setTimer = setInterval(() => {
      if (timer <= 0) {
        clearInterval(setTimer);
        this.showResendBtn = true;
      } else {
        const minutes = Math.floor(timer / 60);
        const seconds = timer % 60;
        this.otpTimer = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
        timer -= 1;
      }
    }, 1000);
  }


  getUser() {
    this.authService.registeredUser$.subscribe((user) => {
      this.user = user;
    });
  }

  verifyOtp() {
    const otp = 123456;
    if (otp == this.otp) {
      this.registerUser();
    }
    else {
      this.toastService.showError('Error', 'Invalid OTP');
    }

  }
  registerUser() {
    if (this.user) {
      const requestModel = {
        firstName: this.user.firstName,
        lastName: this.user.lastName,
        email: this.user.email,
        mobile: this.user.mobile,
        pin: this.user.pin,
      }
      this.pharmacyMerchantService.registerUser(requestModel)
        .pipe(first())
        .subscribe({
          next: () => {
            this.toastService.showSuccess('Success', 'User registered successfully');
            this.router.navigate(['/']);
          },
          error: error => {
          }
        });
    }
  }
  beforeUnloadHandler(event: BeforeUnloadEvent) {
    const confirmationMessage = 'Are you sure you want to leave? Changes you made may not be saved.';
    event.returnValue = confirmationMessage;
    return confirmationMessage;
  }

  @HostListener('window:beforeunload', ['$event'])
  handleBeforeUnload(event: BeforeUnloadEvent): void {
    this.beforeUnloadHandler(event);
  }


}
