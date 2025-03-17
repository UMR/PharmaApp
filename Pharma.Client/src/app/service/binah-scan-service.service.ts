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
    const URI = `${environment.apiUrl}/v1/Pharmacy/GetQRCode`;
    return this.http.get(URI, { observe: 'response' });
  }
}
