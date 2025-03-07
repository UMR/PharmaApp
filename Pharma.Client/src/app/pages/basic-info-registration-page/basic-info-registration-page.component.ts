import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-basic-info-registration-page',
  standalone: false,
  templateUrl: './basic-info-registration-page.component.html',
  styleUrl: './basic-info-registration-page.component.css'
})
export class BasicInfoRegistrationPageComponent {

  /**
   *
   */
  constructor(private router: Router) {

  }

  login() {
    this.router.navigate(['/vital-scan']);
  }

}


