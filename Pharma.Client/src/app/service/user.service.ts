import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  apiUrl: any = environment.apiUrl;

  getAllUsers(pageNumber: number, pageSize: number, searchText: string) {
    const URI = `${environment.apiUrl}/v1/Admin/GetAllUsers?search=${searchText}&page=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get(URI, { observe: 'response' });
  }

  updateUserStatus(userId: string, status: string) {
    const URI = `${environment.apiUrl}/v1/Admin/UpdateUserStatus/${userId}/${status}`;
    return this.http.put(URI, {});
  }
}
