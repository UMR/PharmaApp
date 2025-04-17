import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../service/auth.service';
import { first } from 'rxjs';
import { PharmacyService } from '../../../service/pharmacy.service';
import { phoneNumberValidator } from '../../../common/Validator/phonenumber.validator';
import { ToastMessageService } from '../../../service/toast-message.service';

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
  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService, private route: ActivatedRoute, private pharmacyService: PharmacyService, private toastService: ToastMessageService) {

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
    var request = {
      firstName: this.userRegistrationForm.value.firstName,
      lastName: this.userRegistrationForm.value.lastName,
      email: this.userRegistrationForm.value.email,
      mobile: this.userRegistrationForm.value.mobile,
      pin: this.userRegistrationForm.value.pin,
    }
    this.pharmacyService.isUserExists(request).subscribe({
      next: (res: any) => {
        if (res.body === false) {
          this.generateOtp('1');
          this.pharmacyService.setUser(this.userRegistrationForm.value);
        }
      },
      error: (err) => {

      }
    });
  }


  generateOtp(arg0: string) {
    let loginId = this.userRegistrationForm.value.email;
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
        firstName: [null, [Validators.required, Validators.minLength(3)]],
        lastName: [null],
        email: [null, Validators.email],
        mobile: ['', [
          Validators.required,
          phoneNumberValidator(this.defaultCountry)
        ]],
        pin: [null, [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
        confirmPin: [null, [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
        termsAccepted: [false, Validators.requiredTrue],
      },
      { validators: this.passwordMatchValidator }
    );
  }
  login() {
    this.router.navigate(['/pharmacy-login']);
  }

  handleKeyDown(event: KeyboardEvent): void {
    console.log(event.key);
    const allowedKeys = ['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight'];
    if (
      allowedKeys.includes(event.key) ||
      (!isNaN(Number(event.key)) && event.key !== ' ')
    ) {
      return;
    } else {
      event.preventDefault();
    }
  }

}
