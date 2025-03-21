import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private http: HttpClient) { }
  apiUrl: string = environment.apiUrl;

  registerCustomer(customer: any) {
    const URI = `${environment.apiUrl}/v1/Customer/Register`;
    return this.http.post(URI, customer);
  }
}
