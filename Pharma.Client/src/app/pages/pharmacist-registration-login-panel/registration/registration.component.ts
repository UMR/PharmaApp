import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../service/auth.service';
import { first } from 'rxjs';
import { PharmacyService } from '../../../service/pharmacy.service';
import { phoneNumberValidator } from '../../../common/Validator/phonenumber.validator';

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
  defaultCountry: string = 'IN'
  /**
   *
   */
  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService, private route: ActivatedRoute, private pharmacyService: PharmacyService) {

  }
  ngOnInit(): void {
    this.currentDate = new Date();
    this.initializeForm();
  }


  onSubmit() {
    this.submitted = true;

    if (this.userRegistrationForm.invalid) {
      return;
    }

    this.loading = true;
    this.generateOtp('1');
    this.pharmacyService.setUser(this.userRegistrationForm.value);
  }


  generateOtp(arg0: string) {
    let loginId = this.userRegistrationForm.value.mobile;
    if (loginId != null) {
      this.authService.generateOtp(loginId, arg0)
        .pipe(first())
        .subscribe({
          next: (res) => {
            this.otpTimer = (res as any).data.expireTimeInSecond;
            this.pharmacyService.setOtpTimer(this.otpTimer);
            this.router.navigate(['/otp-verification']);
          },
          error: () => {
            this.loading = false;
          }
        });
    }
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
        mobile: ['', [
          Validators.required,
          phoneNumberValidator(this.defaultCountry)
        ]],
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
