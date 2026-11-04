import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  employeeLogin(username: string, password: string) {
    return this.http.post<any>(
      this.apiUrl + '/login',
      {
        username: username,
        password: password
      }
    );
  }

  managerLogin(username: string, password: string) {
    return this.http.post<any>(
      this.apiUrl + '/managers/login',
      {
        username: username,
        password: password
      }
    );
  }
}