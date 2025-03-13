import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PharmacyMerchantService } from '../../../service/pharmacy-merchant.service';
import { first } from 'rxjs';
import { AuthenticationService } from '../../../service/authentication.service';

@Component({
  selector: 'app-registration',
  standalone: false,
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {

  userRegistrationForm: FormGroup | any;
  currentDate: any;
  loading = false;
  submitted = false;
  otpTimer: any;
  /**
   *
   */
  constructor(private router: Router, private fb: FormBuilder, private pharmacyMerchantService: PharmacyMerchantService, private route: ActivatedRoute, private authService: AuthenticationService) {

  }
  ngOnInit(): void {
    this.currentDate = new Date();
    this.initializeForm();
  }


  onSubmit() {
    debugger;
    this.submitted = true;

    if (this.userRegistrationForm.invalid) {
      return;
    }

    this.loading = true;
    this.generateOtp('1');
    this.NavigateToOtpPage();
    this.authService.setUser(this.userRegistrationForm.value);
  }


  generateOtp(arg0: string) {
    let loginId = this.userRegistrationForm.value.mobile;
    if (loginId != null) {
      this.pharmacyMerchantService.generateOtp(loginId, arg0)
        .pipe(first())
        .subscribe({
          next: (res) => {
            this.otpTimer = (res as any).data.expireTimeInSecond;
            this.authService.setOtpTimer(this.otpTimer);
            this.router.navigate(['/otp-verification']);
          },
          error: () => {
            this.loading = false;
          }
        });
    }
  }
  NavigateToOtpPage() {
    this.router.navigate(['/otp-verification']);
  }

  passwordMatchValidator(control: AbstractControl) {
    const pin = control.get('pin');
    const confirmPin = control.get('confirmPin');

    if (!pin || !confirmPin) return null;

    if (confirmPin.value !== pin.value) {
      confirmPin.setErrors({ mismatch: true });
    } else {
      confirmPin.setErrors(null);
    }
    return null;
  }

  get f() { return this.userRegistrationForm.controls; }

  initializeForm() {
    this.userRegistrationForm = this.fb.group(
      {
        firstName: [null, Validators.required],
        lastName: [null],
        email: [null, Validators.email],
        mobile: [null, Validators.required],
        pin: [null, Validators.required],
        confirmPin: [null, Validators.required],
        termsAccepted: [false, Validators.requiredTrue],
      },
      { validators: this.passwordMatchValidator }
    );
  }
  login() {
    this.router.navigate(['/pharmacy-login']);
  }
}
