import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private http: HttpClient) { }
  apiUrl: string = 'https://umrtest.com/pharmaAuthApi/api/v1';
  // apiUrl: string = 'http://localhost:7001/api/v1';


  verifyToken(token: string, pharmacyId: string, customerId: string): Observable<any> {
    const URI = this.apiUrl + `/payment/verifyToken/${pharmacyId}/${customerId}?token=${token}`;
    return this.http.get(URI, { observe: 'response' });
  }
}
