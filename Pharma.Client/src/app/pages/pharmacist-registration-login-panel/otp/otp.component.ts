import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../service/authentication.service';
import { PharmacyMerchantService } from '../../../service/pharmacy-merchant.service';
import { first, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { ToastMessageService } from '../../../service/toast-message.service';
import { forgotPin } from '../../../common/constant/auth-key';

@Component({
  selector: 'app-otp',
  standalone: false,
  templateUrl: './otp.component.html',
  styleUrl: './otp.component.css'
})
export class OtpComponent implements OnInit, OnDestroy {
  /**
   *
   */
  constructor(private authService: AuthenticationService, private pharmacyMerchantService: PharmacyMerchantService, private router: Router, private toastService: ToastMessageService) {

  }

  ngOnDestroy(): void {
    this._otpTimer.unsubscribe();
  }
  _otpTimer: Subscription | any;
  otpTimer: any;;
  public showResendBtn: boolean = false;
  user: any;
  otp: any;

  ngOnInit(): void {
    this.getUser();
    this._otpTimer = this.authService.otpTimer$.subscribe((timer) => this.otpTimer = this.getTimer(timer));
  }



  getTimer(timer: number) {
    console.log(timer);
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
    if (this.otp) {
      const model = {
        loginId: this.user.mobile,
        otp: this.otp,
        otpType: '1'
      }
      this.pharmacyMerchantService.verifyOtp(model)
        .pipe(first())
        .subscribe({
          next: (res) => {
            if (res.status == 200) {
              this.registerUser();
            }
            this.toastService.showSuccess('Success', 'OTP verified successfully');

          },
          error: () => {
            this.toastService.showError('Error', 'Invalid OTP');
          }
        });
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

  resendOtp() {
    var forgotPinValue = localStorage.getItem(forgotPin);
    var otpType = "1";
    if (forgotPinValue) {
      otpType = "2";
    }
    this.pharmacyMerchantService.generateOtp(this.user.mobile, otpType).subscribe({
      next: (res) => {
        if ((res as any).isSuccessful) {
          this.toastService.showSuccess("Success", "OTP Resent successfully!");
          this.otpTimer = (res as any).data.expireTimeInSecond;
          this.startTimer(this.otpTimer);
          this.showResendBtn = false;
        }
      },
      error: (err) => {
        console.log(err);
      }
    });
  }


  startTimer(otpTimer: any) {
    this.getTimer(otpTimer);
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
