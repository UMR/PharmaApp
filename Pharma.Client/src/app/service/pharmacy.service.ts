import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { authCookieKey } from '../common/constant/auth-key';

@Injectable({
  providedIn: 'root'
})
export class PharmacyService {
  private registeredUser = new BehaviorSubject<any>(null);
  registeredUser$ = this.registeredUser.asObservable();
  private otpTimer = new BehaviorSubject<any>('');
  otpTimer$ = this.otpTimer.asObservable();
  apiUrl: string = environment.apiUrl;
  private pharmacyData = new BehaviorSubject<any>(null);

  constructor(private http: HttpClient) { }

  setUser(user: any) {
    this.registeredUser.next(user);
  }
  setOtpTimer(timer: any) {
    this.otpTimer.next(timer);
  }
  setPharmacyData(data: any) {
    this.pharmacyData.next(data);
  }

  getPharmacyData(): Observable<any> {
    return this.pharmacyData.asObservable();
  }

  hasRole(requiredRoles: string[]): boolean {
    const user = JSON.parse(localStorage.getItem(authCookieKey)!) || null;
    const token = user ? user.accessToken : null;
    if (token) {
      let decryptedToken = this.decryptJwtToken(token);
      const roles = decryptedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      if (!roles) return false;

      if (Array.isArray(roles)) {
        return requiredRoles.some(role => roles.includes(role));
      }
      return requiredRoles.includes(roles);
    }
    return false;
  }


  decryptJwtToken(token: any) {
    const payload = token.split('.')[1];
    const decodedPayload = atob(payload);
    return JSON.parse(decodedPayload);
  }


  getPharmacy() {
    const URI = `${environment.apiUrl}/v1/Pharmacy`;
    return this.http.get(URI, { observe: 'response' });
  }
  getPharmacyByUniqueId(uniqueId: string) {
    const URI = `${environment.apiUrl}/v1/Pharmacy/Url/${uniqueId}`;
    return this.http.get(URI, { observe: 'response' });
  }
  getCurrentUser() {
    const URI = `${environment.apiUrl}/v1/User/Get`;
    return this.http.get(URI, { observe: 'response' });
  }

  isUserExists(request: any) {
    const URI = `${environment.apiUrl}/v1/User/IsUserExists`;
    return this.http.post(URI, request, { observe: 'response' });
  }

}
