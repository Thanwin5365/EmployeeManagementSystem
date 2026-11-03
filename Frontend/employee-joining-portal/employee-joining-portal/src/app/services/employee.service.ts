import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = 'https://localhost:7024';

  constructor(private http: HttpClient) {}

  getEmployeeById(employeeId: number) {
    return this.http.get<any>(
      this.apiUrl + '/employees/' + employeeId
    );
  }

  getAllEmployees() {
    return this.http.get<any>(
      this.apiUrl + '/employees'
    );
  }

  getReportsByEmployee(employeeId: number) {
    return this.http.get<any[]>(
      this.apiUrl + '/joining-reports/employee/' + employeeId
    );
  }

  getJoiningReportSummary(employeeId: number) {

    return this.getReportsByEmployee(employeeId).pipe(

      map((reports: any[]) => {

        const total = reports.length;

        const accepted = reports.filter(
          report =>
            (report.approvalStatus || '').toLowerCase() === 'approved'
        ).length;

        const rejected = reports.filter(
          report =>
            (report.approvalStatus || '').toLowerCase() === 'rejected'
        ).length;

        const pending = reports.filter(
          report =>
            (report.approvalStatus || '').toLowerCase() === 'pending'
        ).length;

        return {
          reports,
          total,
          accepted,
          rejected,
          pending
        };

      })

    );

  }

  createJoiningReport(joiningReport: any) {
    return this.http.post<any>(
      this.apiUrl + '/joining-reports',
      joiningReport
    );
  }
}