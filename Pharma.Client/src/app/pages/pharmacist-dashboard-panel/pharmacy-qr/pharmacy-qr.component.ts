import { Component, OnInit } from '@angular/core';
import { PharmacyMerchantService } from '../../../service/pharmacy-merchant.service';
import { BinahScanService } from '../../../service/binah-scan-service.service';

@Component({
  selector: 'app-pharmacy-qr',
  standalone: false,
  templateUrl: './pharmacy-qr.component.html',
  styleUrl: './pharmacy-qr.component.css'
})
export class PharmacyQrComponent implements OnInit {
  constructor(private binahScanService: BinahScanService) { }

  qrCodeImage: any;

  ngOnInit(): void {
    this.getPharmacyUser();
    this.getQrCode();
  }
  getPharmacyUser() {

  }
  getQrCode() {
    this.binahScanService.getQrCode().subscribe({
      next: (res) => {
        this.qrCodeImage = `data:image/png;base64,${res}`;
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

}
