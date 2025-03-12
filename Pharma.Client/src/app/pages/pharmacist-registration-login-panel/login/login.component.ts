import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../../service/authentication.service';
import { PharmacyMerchantService } from '../../../service/pharmacy-merchant.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  userloginForm: FormGroup | any;
  currentDate: any;
  /**
   *
   */
  constructor(private router: Router, private fb: FormBuilder, private merchantService: PharmacyMerchantService) {

  }
  ngOnInit(): void {
    this.initializeForm();
  }

  get f() { return this.userloginForm.controls; }
  login() {
    this.router.navigate(['/vital-scan']);
  }

  initializeForm() {
    this.userloginForm = this.fb.group({
      loginId: [null, Validators.required],
      pin: [null, Validators.required]
    });
  }
  register() {
    this.router.navigate(['/pharmacy-registration']);
  }
}

