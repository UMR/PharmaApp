import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {


  constructor(private http: HttpClient) { }
  apiUrl: string = environment.apiUrl;

  getPackageInfo() {
    const URI = `${environment.apiUrl}/v1/Package/latest`;
    return this.http.get(URI, { observe: 'response' });
  }
  getKey(): Observable<any> {
    var getCurrentUserURI = `${environment.apiUrl}/v1/payment/getkey`;
    return this.http.get(getCurrentUserURI, { observe: 'response' });
  }

  createOrder(packageId: any, currency: any): Observable<any> {
    var getCurrentUserURI = `${environment.apiUrl}/v1/payment/create/order?packageId=${packageId}&currencyCode=${currency}`;
    return this.http.get(getCurrentUserURI, { observe: 'response' });
  }

}
