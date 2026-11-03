import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

interface JoiningReport {
  id: number;

  employeeId: number;
  employeeName: string;

  designation?: string;
  department?: string;

  joiningType?: string;

  joinedDutyOn?: string;
  joiningDutyOn?: string;

  formDate?: string;

  leaveAppliedTill?: string;

  delayReason?: string;

  salary?: number;

  approvalStatus?: string;

  level1ManagerId?: number | null;
  level1ApprovedDate?: string | null;
  level1RejectionReason?: string | null;

  level2ManagerId?: number | null;
  level2ApprovedDate?: string | null;
  level2RejectionReason?: string | null;

  hrManagerId?: number | null;
  hrApprovedDate?: string | null;
  hrRejectionReason?: string | null;

  createdDate?: string;
}

@Component({
  selector: 'app-hr-home',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    DatePipe
  ],

  templateUrl: './hr-home.html',
  styleUrl: './hr-home.css'
})
export class HrHome implements OnInit {

  // =====================================================
  // BACKEND
  // =====================================================

  private readonly apiUrl =
    'https://localhost:7024/joining-reports';


  // =====================================================
  // HR INFORMATION
  // =====================================================

  hrManagerId: number = 401;

  hrManagerName: string = 'Anitha';


  // =====================================================
  // DATA
  // =====================================================

  pendingReports: JoiningReport[] = [];

  selectedReport: JoiningReport | null = null;


  // =====================================================
  // UI STATE
  // =====================================================

  loading: boolean = false;

  actionLoading: boolean = false;

  errorMessage: string = '';

  successMessage: string = '';


  // =====================================================
  // MODALS
  // =====================================================

  showPendingRequests: boolean = false;

  showReport: boolean = false;


  // =====================================================
  // REJECTION
  // =====================================================

  rejectionReason: string = '';


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    console.log('HR HOME INITIALIZED');

    this.loadHrSession();

    this.loadPendingReports();

  }


  // =====================================================
  // LOAD HR SESSION
  // =====================================================

  loadHrSession(): void {

    const storedHrId =
      sessionStorage.getItem('hrManagerId');

    const storedHrName =
      sessionStorage.getItem('hrManagerName');


    if (storedHrId) {

      this.hrManagerId =
        Number(storedHrId);

    }


    if (storedHrName) {

      this.hrManagerName =
        storedHrName;

    }


    console.log(
      'HR Manager ID:',
      this.hrManagerId
    );

    console.log(
      'HR Manager Name:',
      this.hrManagerName
    );

  }


  // =====================================================
  // LOAD PENDING HR REPORTS
  // =====================================================

  loadPendingReports(): void {

    this.loading = true;

    this.errorMessage = '';

    console.log(
      'Loading HR pending joining reports...'
    );


    const url =
      `${this.apiUrl}/pending-hr`;


    console.log(
      'Request URL:',
      url
    );


    this.http
      .get<JoiningReport[]>(url)
      .subscribe({

        // ===============================================
        // SUCCESS
        // ===============================================

        next: (response: JoiningReport[]) => {

          console.log(
            'HR pending reports:',
            response
          );


          if (Array.isArray(response)) {

            this.pendingReports =
              response;

          } else {

            this.pendingReports = [];

          }


          /*
           * IMPORTANT:
           * Stop loading after the backend response
           * has been received.
           */

          this.loading = false;


          console.log(
            'Reports loaded:',
            this.pendingReports.length
          );

          console.log(
            'Loading state:',
            this.loading
          );

        },


        // ===============================================
        // ERROR
        // ===============================================

        error: (error: HttpErrorResponse) => {

          console.error(
            'Unable to load HR pending reports:',
            error
          );


          this.pendingReports = [];

          this.loading = false;


          this.errorMessage =
            this.getErrorMessage(
              error,
              'Unable to load pending joining reports.'
            );


          console.log(
            'Loading state after error:',
            this.loading
          );

        }

      });

  }


  // =====================================================
  // OPEN PENDING REQUESTS
  // =====================================================

  openPendingRequests(): void {

    if (this.actionLoading) {
      return;
    }


    this.errorMessage = '';

    this.successMessage = '';

    this.showPendingRequests = true;


    /*
     * Load the reports again when HR opens
     * the pending request window.
     */

    this.loadPendingReports();

  }


  // =====================================================
  // CLOSE PENDING REQUESTS
  // =====================================================

  closePendingRequests(): void {

    if (this.actionLoading) {
      return;
    }


    this.showPendingRequests = false;

  }


  // =====================================================
  // VIEW REPORT
  // =====================================================

  viewReport(
    report: JoiningReport
  ): void {

    console.log(
      'Selected HR joining report:',
      report
    );


    this.selectedReport =
      report;


    this.rejectionReason = '';

    this.errorMessage = '';

    this.successMessage = '';


    this.showReport = true;

    this.showPendingRequests = false;

  }


  // =====================================================
  // CLOSE REPORT
  // =====================================================

  closeReport(): void {

    if (this.actionLoading) {
      return;
    }


    this.showReport = false;

    this.selectedReport = null;

    this.rejectionReason = '';

  }


  // =====================================================
  // APPROVE REPORT
  // =====================================================

  approveReport(): void {

    if (this.actionLoading) {
      return;
    }


    if (!this.selectedReport) {

      this.errorMessage =
        'No joining report selected.';

      return;

    }


    const report =
      this.selectedReport;


    const confirmed =
      window.confirm(
        `Are you sure you want to finally approve the joining report for ${report.employeeName}?`
      );


    if (!confirmed) {
      return;
    }


    this.actionLoading = true;

    this.errorMessage = '';

    this.successMessage = '';


    console.log(
      '================================'
    );

    console.log(
      'HR FINAL APPROVAL'
    );

    console.log(
      'Report ID:',
      report.id
    );

    console.log(
      'Employee ID:',
      report.employeeId
    );

    console.log(
      'HR Manager ID:',
      this.hrManagerId
    );

    console.log(
      '================================'
    );


    const body = {

      approvedBy:
        this.hrManagerId

    };


    const url =
      `${this.apiUrl}/${report.id}/hr/approve`;


    console.log(
      'Approval URL:',
      url
    );


    this.http
      .put(
        url,
        body
      )
      .subscribe({

        // =============================================
        // APPROVAL SUCCESS
        // =============================================

        next: (response) => {

          console.log(
            'HR approval successful:',
            response
          );


          this.actionLoading = false;


          this.showReport = false;

          this.showPendingRequests = false;

          this.selectedReport = null;

          this.rejectionReason = '';


          this.successMessage =
            'Joining report has been finally approved by HR.';


          /*
           * Reload the pending list.
           */

          this.loadPendingReports();

        },


        // =============================================
        // APPROVAL ERROR
        // =============================================

        error: (error: HttpErrorResponse) => {

          console.error(
            'HR approval failed:',
            error
          );


          this.actionLoading = false;


          this.errorMessage =
            this.getErrorMessage(
              error,
              'Unable to approve joining report.'
            );

        }

      });

  }


  // =====================================================
  // REJECT REPORT
  // =====================================================

  rejectReport(): void {

    if (this.actionLoading) {
      return;
    }


    if (!this.selectedReport) {

      this.errorMessage =
        'No joining report selected.';

      return;

    }


    const reason =
      this.rejectionReason.trim();


    if (!reason) {

      this.errorMessage =
        'Please enter a rejection reason.';

      return;

    }


    const report =
      this.selectedReport;


    const confirmed =
      window.confirm(
        `Are you sure you want to reject the joining report for ${report.employeeName}?`
      );


    if (!confirmed) {
      return;
    }


    this.actionLoading = true;

    this.errorMessage = '';

    this.successMessage = '';


    console.log(
      '================================'
    );

    console.log(
      'HR REJECTION'
    );

    console.log(
      'Report ID:',
      report.id
    );

    console.log(
      'Employee ID:',
      report.employeeId
    );

    console.log(
      'HR Manager ID:',
      this.hrManagerId
    );

    console.log(
      'Rejection Reason:',
      reason
    );

    console.log(
      '================================'
    );


    const body = {

      managerId:
        this.hrManagerId,

      reason:
        reason

    };


    const url =
      `${this.apiUrl}/${report.id}/hr/reject`;


    console.log(
      'Rejection URL:',
      url
    );


    this.http
      .put(
        url,
        body
      )
      .subscribe({

        // =============================================
        // REJECTION SUCCESS
        // =============================================

        next: (response) => {

          console.log(
            'HR rejection successful:',
            response
          );


          this.actionLoading = false;


          this.showReport = false;

          this.showPendingRequests = false;

          this.selectedReport = null;

          this.rejectionReason = '';


          this.successMessage =
            'Joining report has been rejected by HR.';


          /*
           * Reload pending reports.
           */

          this.loadPendingReports();

        },


        // =============================================
        // REJECTION ERROR
        // =============================================

        error: (error: HttpErrorResponse) => {

          console.error(
            'HR rejection failed:',
            error
          );


          this.actionLoading = false;


          this.errorMessage =
            this.getErrorMessage(
              error,
              'Unable to reject joining report.'
            );

        }

      });

  }


  // =====================================================
  // REFRESH
  // =====================================================

  refresh(): void {

    if (this.loading) {
      return;
    }


    this.errorMessage = '';

    this.successMessage = '';


    this.loadPendingReports();

  }


  // =====================================================
  // LOGOUT
  // =====================================================

  logout(): void {

    sessionStorage.removeItem(
      'hrManagerId'
    );

    sessionStorage.removeItem(
      'hrManagerName'
    );

    sessionStorage.removeItem(
      'managerId'
    );

    sessionStorage.removeItem(
      'managerName'
    );

    sessionStorage.removeItem(
      'managerKey'
    );

    sessionStorage.removeItem(
      'managerLevel'
    );

    sessionStorage.removeItem(
      'employeeId'
    );

    sessionStorage.removeItem(
      'employeeName'
    );


    this.router.navigate([
      '/login'
    ]);

  }


  // =====================================================
  // ERROR MESSAGE
  // =====================================================

  private getErrorMessage(
    error: HttpErrorResponse,
    fallback: string
  ): string {

    if (error?.error) {

      if (
        typeof error.error === 'object' &&
        error.error.message
      ) {

        return error.error.message;

      }


      if (
        typeof error.error === 'string' &&
        error.error.trim()
      ) {

        return error.error;

      }

    }


    if (error.status === 0) {

      return 'Unable to connect to the backend server.';

    }


    if (error.status === 401) {

      return 'You are not authorized to perform this action.';

    }


    if (error.status === 404) {

      return 'The requested API endpoint was not found.';

    }


    if (error.status >= 500) {

      return 'The backend server encountered an error.';

    }


    return fallback;

  }

}