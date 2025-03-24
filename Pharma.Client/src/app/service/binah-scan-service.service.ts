import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BinahScanService {

  constructor(private http: HttpClient) { }
  apiUrl: string = environment.apiUrl;

  getQrCode() {
    const URI = `${environment.apiUrl}/v1/Pharmacy/QRCode`;
    return this.http.get(URI, { observe: 'response' });
  }
  pharmacyRegistration(pharmacy: any) {
    const URI = `${environment.apiUrl}/v1/Pharmacy/Create`;
    return this.http.post(URI, pharmacy);
  }
}
