import { Component, OnInit } from '@angular/core';
import { customerInfo } from '../../../common/constant/auth-key';

@Component({
  selector: 'app-pay-now',
  standalone: false,
  templateUrl: './pay-now.component.html',
  styleUrl: './pay-now.component.css'
})
export class PayNowComponent implements OnInit {

  ngOnInit(): void {
    this.getUser();
  }

  getUser() {
    this.user = JSON.parse(localStorage.getItem(customerInfo)!);

  }
  user: any;
  displayModal: boolean = false;
  scanCount: number = 1;
  amount: number = 100;

  payNowmodal() {
    this.displayModal = true;
  }
  payNow() {

  }

}
