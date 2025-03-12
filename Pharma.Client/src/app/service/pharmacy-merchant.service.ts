import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.production';
import { BehaviorSubject, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PharmacyMerchantService {
  apiUrl: string = environment.apiUrl;
  private userSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  public user: Observable<null> | undefined;


  constructor(private http: HttpClient) {
    this.userSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
    this.user = this.userSubject.asObservable();
  }

  registerUser(user: any) {
    return this.http.post(this.apiUrl + '/Auth/Register', user);
  }
  public get userValue() {
    return this.userSubject.value;
  }

  generateOtp(user: any) {
    return this.http.post(this.apiUrl + '/Auth/generate-otp', user);
  }
  login(user: any) {
    return this.http.post<any>(`${environment.apiUrl}/Auth/Login`, user)
      .pipe(map(user => {
        localStorage.setItem('user', JSON.stringify(user));
        this.userSubject.next(user);
        return user;
      }));
  }
}
