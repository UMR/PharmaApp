import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { phoneNumberValidator } from '../../common/Validator/phonenumber.validator';
import { CustomerService } from '../../service/customer.service';
import { customerInfo } from '../../common/constant/auth-key';

@Component({
  selector: 'app-basic-info-registration-page',
  standalone: false,
  templateUrl: './basic-info-registration-page.component.html',
  styleUrl: './basic-info-registration-page.component.css'
})
export class BasicInfoRegistrationPageComponent implements OnInit {
  userRegistrationForm: FormGroup | any;
  currentDate: any;
  pharmacy: any;
  defaultCountry: string = 'IN'
  submitted = false;
  /**
   *
   */
  constructor(private router: Router, private fb: FormBuilder, private route: ActivatedRoute, private customerService: CustomerService) {

  }
  ngOnInit(): void {
    this.currentDate = new Date();
    this.route.data.subscribe((data) => {
      this.pharmacy = data['pharmacy'];

    });

    this.initializeForm();
  }

  get f() { return this.userRegistrationForm.controls; }

  customerLogin() {
    this.submitted = true;

    if (this.userRegistrationForm.invalid) {
      return;
    }
    this.customerService.registerCustomer(this.userRegistrationForm.value).subscribe({
      next: (res: any) => {
        localStorage.setItem(customerInfo, JSON.stringify({ ...res, pharmacyId: this.pharmacy.body.id }));
        this.router.navigate(['/pay-now']);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  initializeForm() {
    this.userRegistrationForm = this.fb.group({
      pharmacyId: [this.pharmacy.body.id],
      firstName: [null, [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      lastName: [null],
      mobile: ["", [Validators.required, phoneNumberValidator(this.defaultCountry)]],
      email: [null, Validators.email],
      weight: [null, Validators.required],
      age: [null, Validators.required],
    });
  }

}


