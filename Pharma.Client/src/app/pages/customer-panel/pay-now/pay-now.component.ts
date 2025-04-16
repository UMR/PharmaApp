import { Component, OnInit } from '@angular/core';
import { customerInfo } from '../../../common/constant/auth-key';
import { PaymentService } from '../../../service/payment.service';
import { ToastMessageService } from '../../../service/toast-message.service';

@Component({
  selector: 'app-pay-now',
  standalone: false,
  templateUrl: './pay-now.component.html',
  styleUrl: './pay-now.component.css'
})
export class PayNowComponent implements OnInit {

  /**
   *
   */
  constructor(private paymentService: PaymentService, private toastService: ToastMessageService) {

  }

  ngOnInit(): void {
    this.getUser();
  }

  getUser() {
    this.user = JSON.parse(localStorage.getItem(customerInfo)!);

  }
  user: any;
  displayModal: boolean = false;
  packageInfo: any;
  apiKey: any;

  payNowmodal() {
    this.displayModal = true;
    this.getPaymentInfo();
  }

  getPaymentInfo() {
    this.paymentService.getPackageInfo().subscribe({
      next: (res) => {
        this.packageInfo = res.body;
        this.loadRazorpayScript();
        this.getKey();
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  loadRazorpayScript(): void {
    const script = document.createElement('script');
    script.src = 'https://checkout.razorpay.com/v1/checkout.js';
    script.defer = true;
    document.body.appendChild(script);
  }

  getKey() {
    this.paymentService.getKey().subscribe({
      next: (res) => {
        this.apiKey = res.body?.key;
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
  payNow() {
    this.createOrder()

  }
  createOrder() {
    const packageId = this.packageInfo.id;
    const currency = this.packageInfo.currencyCode;
    this.paymentService.createOrder(packageId, currency).subscribe({
      next: (res) => {
        this.pay(res.body);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  pay(body: any) {
    const options = {
      key: this.apiKey,
      amount: this.packageInfo.price,
      currency: this.packageInfo.currencyCode,
      name: 'SCOHN™',
      description: this.packageInfo.name,
      order_id: body.orderId,
      prefill: {
        name: this.user.firstName + ' ' + this.user.lastName,
        contact: this.user.mobile,
        email: this.user.email
      },
      theme: {
        color: '#660033'
      },
      handler: (response: any) => {
        var request = {
          orderId: response.razorpay_order_id,
          paymentId: response.razorpay_payment_id,
          signature: response.razorpay_signature,
          customerId: this.user.id,
          pharmacyId: this.user.pharmacyId,
          packageId: this.packageInfo.id,
        }
        this.paymentService.createPayment(request).subscribe({
          next: (res) => {
            this.toastService.showSuccess("success", 'Payment successful');
            this.displayModal = false;

            // window.location.href = '/binah';
          },
          error: (err) => {
            this.toastService.showError("failed", 'Payment failed');
            // window.location.href = '/binah';
          }
        });

      },
      modal: {
        ondismiss: () => {
          this.toastService.showError("failed", 'Payment dismissed by User');
        }
      },
      onerror: () => {
        this.toastService.showError("failed", 'Payment failed');
      },

    }
    const rzp = new (window as any).Razorpay(options);
    rzp.open();

  }

}
