import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.production';

@Injectable({
  providedIn: 'root'
})
export class PharmacyMerchantService {
  apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  registerUser(user: any) {
    return this.http.post(this.apiUrl + '/pharmacy-merchant/register', user);
  }

  generateOtp(user: any) {
    return this.http.post(this.apiUrl + '/pharmacy-merchant/generate-otp', user);
  }
}
