import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7024';

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