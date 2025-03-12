import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-basic-info-registration-page',
  standalone: false,
  templateUrl: './basic-info-registration-page.component.html',
  styleUrl: './basic-info-registration-page.component.css'
})
export class BasicInfoRegistrationPageComponent implements OnInit {
  userRegistrationForm: FormGroup | any;
  currentDate: any;
  /**
   *
   */
  constructor(private router: Router, private fb: FormBuilder) {

  }
  ngOnInit(): void {
    this.currentDate = new Date();
    this.initializeForm();
  }


  login() {
    this.router.navigate(['/vital-scan']);
  }

  initializeForm() {
    this.userRegistrationForm = this.fb.group({
      name: [null],
      phoneNumber: [null],
      email: [null],
      weight: [null],
      dateOfBirth: [null],
    });
  }

}


