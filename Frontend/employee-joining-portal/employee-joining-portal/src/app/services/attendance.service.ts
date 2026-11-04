
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface AttendanceResponse {
  attendanceId: number;
  employeeId: number;
  attendanceDate: string;
  finalCheckIn: string | null;
  finalCheckOut: string | null;
  totalWorkingHours: number;
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private readonly apiUrl = `${environment.apiUrl}/api/Attendance`;

  constructor(private http: HttpClient) {}

  getTodayAttendance(
    employeeId: number
  ): Observable<AttendanceResponse | null> {
    return this.http.get<AttendanceResponse | null>(
      `${this.apiUrl}/${employeeId}`
    );
  }

  checkIn(employeeId: number): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/check-in/${employeeId}`,
      {}
    );
  }

  checkOut(employeeId: number): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/check-out/${employeeId}`,
      {}
    );
  }
}
