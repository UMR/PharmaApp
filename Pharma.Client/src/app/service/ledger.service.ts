import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LedgerService {

  constructor(private http: HttpClient) { }
  apiUrl: string = environment.apiUrl;

  getTransactionDetails(fromDate: string, toDate: string, pageIndex: string, pageSize: string, ledgerType: string) {
    const URI = `${this.apiUrl}/TransactionDetails/${ledgerType}/${fromDate}/${toDate}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get(URI, { observe: 'response' });
  }
}
