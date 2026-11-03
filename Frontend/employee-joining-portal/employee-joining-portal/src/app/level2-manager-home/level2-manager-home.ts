import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { timeout, finalize } from 'rxjs';


interface JoiningReport {

  id: number;

  employeeId: number;

  employeeName: string;

  designation: string;

  department: string;

  joinedDutyOn: string;

  joiningType: string;

  leaveAppliedTill: string;

  delayReason: string;

  formDate: string;

  signature: string;

  createdDate: string;

  approvalStatus: string;

  approvedBy: number | null;

  approvedDate: string | null;

  rejectionReason: string | null;

  level1ManagerId: number | null;

  level2ManagerId: number | null;

}


@Component({

  selector: 'app-level2-manager-home',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './level2-manager-home.html',

  styleUrl: './level2-manager-home.css'

})


export class Level2ManagerHome implements OnInit {


  private apiUrl = 'https://localhost:7024';


  managerId: number = 0;

  managerName: string = '';


  reports: JoiningReport[] = [];

  selectedReport: JoiningReport | null = null;


  showPendingRequests: boolean = false;

  showReportModal: boolean = false;


  loading: boolean = false;

  actionLoading: boolean = false;


  errorMessage: string = '';

  successMessage: string = '';


  rejectionReason: string = '';


  constructor(

    private http: HttpClient,

    private cdr: ChangeDetectorRef

  ) {}


  // =====================================================
  // INITIALIZE
  // =====================================================

  ngOnInit(): void {

    console.log('Level 2 Dashboard Loaded');


    const storedManagerId =
      sessionStorage.getItem('managerId');


    const storedManagerName =
      sessionStorage.getItem('managerName');


    console.log(
      'Stored Manager ID:',
      storedManagerId
    );


    console.log(
      'Stored Manager Name:',
      storedManagerName
    );


    if (!storedManagerId) {

      this.errorMessage =
        'Manager session not found. Please login again.';

      this.cdr.detectChanges();

      return;

    }


    this.managerId = Number(storedManagerId);

    this.managerName =
      storedManagerName || '';


    console.log(
      'Final Manager ID:',
      this.managerId
    );


    this.loadPendingReports();

  }


  // =====================================================
  // LOAD LEVEL 2 PENDING REPORTS
  // =====================================================

  loadPendingReports(): void {

    this.loading = true;

    this.errorMessage = '';

    this.reports = [];


    const url =
      `${this.apiUrl}/joining-reports/pending-level2/${this.managerId}`;


    console.log('=================================');

    console.log('Calling Level 2 Pending API');

    console.log(
      'Manager ID:',
      this.managerId
    );

    console.log(
      'URL:',
      url
    );

    console.log('=================================');


    this.http

      .get<JoiningReport[]>(url)

      .pipe(

        timeout(10000),

        finalize(() => {

          this.loading = false;

          this.cdr.detectChanges();

        })

      )

      .subscribe({

        next: (response) => {

          console.log(
            'SUCCESS - Level 2 reports:',
            response
          );


          console.log(
            'Number of reports:',
            response.length
          );


          this.reports = response;

          this.cdr.detectChanges();

        },


        error: (error) => {

          console.error(
            'ERROR - Level 2 reports:',
            error
          );


          if (error.name === 'TimeoutError') {

            this.errorMessage =
              'The server took too long to respond.';

          }

          else if (error.status === 0) {

            this.errorMessage =
              'Unable to connect to the backend server.';

          }

          else if (error.status === 404) {

            this.errorMessage =
              'Level 2 reports API was not found.';

          }

          else if (error.status >= 500) {

            this.errorMessage =
              'Backend server error while loading reports.';

          }

          else {

            this.errorMessage =
              'Unable to load pending joining reports.';

          }


          this.cdr.detectChanges();

        }

      });

  }


  // =====================================================
  // OPEN PENDING REQUESTS POPUP
  // =====================================================

  openPendingRequests(): void {

    this.showPendingRequests = true;

    this.successMessage = '';

    this.errorMessage = '';

    this.loadPendingReports();

  }


  // =====================================================
  // CLOSE PENDING REQUESTS POPUP
  // =====================================================

  closePendingRequests(): void {

    this.showPendingRequests = false;

  }


  // =====================================================
  // OPEN EMPLOYEE REPORT
  // =====================================================

  openReport(report: JoiningReport): void {

    this.selectedReport = report;

    this.showReportModal = true;

    this.rejectionReason = '';

    this.successMessage = '';

    this.errorMessage = '';

  }


  // =====================================================
  // CLOSE EMPLOYEE REPORT
  // =====================================================

  closeReport(): void {

    if (this.actionLoading) {
      return;
    }


    this.showReportModal = false;

    this.selectedReport = null;

    this.rejectionReason = '';

    this.successMessage = '';

    this.errorMessage = '';

  }


  // =====================================================
  // APPROVE LEVEL 2
  // =====================================================

  approveRequest(): void {

    if (!this.selectedReport) {

      return;

    }


    if (this.actionLoading) {

      return;

    }


    const reportId =
      this.selectedReport.id;


    const employeeName =
      this.selectedReport.employeeName;


    const url =
      `${this.apiUrl}/joining-reports/${reportId}/approve-level2`;


    const requestBody = {

      approvedBy: this.managerId

    };


    console.log(
      'Approving Level 2 joining request:',
      reportId
    );


    console.log(
      'Approve URL:',
      url
    );


    console.log(
      'Approve body:',
      requestBody
    );


    this.actionLoading = true;

    this.errorMessage = '';

    this.successMessage = '';


    this.http

      .put<JoiningReport>(

        url,

        requestBody

      )

      .pipe(

        timeout(10000),

        finalize(() => {

          this.actionLoading = false;

          this.cdr.detectChanges();

        })

      )

      .subscribe({

        next: (response) => {

          console.log(
            'Level 2 approval successful:',
            response
          );


          this.successMessage =
            `Joining request #${reportId} for ${employeeName} has been approved.`;


          alert(
            `Joining request #${reportId} for ${employeeName} has been approved.`
          );


          // Remove approved request from pending list
          this.reports =
            this.reports.filter(
              report => report.id !== reportId
            );


          this.showReportModal = false;

          this.selectedReport = null;

          this.rejectionReason = '';


          this.cdr.detectChanges();

        },


        error: (error) => {

          console.error(
            'Level 2 approval failed:',
            error
          );


          console.error(
            'Status:',
            error.status
          );


          console.error(
            'Response:',
            error.error
          );


          if (error.status === 400) {

            this.errorMessage =
              error.error ||
              'The server rejected the approval request.';

          }

          else if (error.status === 404) {

            this.errorMessage =
              'Joining request was not found.';

          }

          else if (error.status === 0) {

            this.errorMessage =
              'Unable to connect to the backend server.';

          }

          else {

            this.errorMessage =
              'Failed to approve the joining request.';

          }


          alert(this.errorMessage);


          this.cdr.detectChanges();

        }

      });

  }


  // =====================================================
  // REJECT LEVEL 2
  // =====================================================

  rejectRequest(): void {

    if (!this.selectedReport) {

      return;

    }


    if (this.actionLoading) {

      return;

    }


    const reason =
      this.rejectionReason.trim();


    // Reason is mandatory
    if (!reason) {

      alert(
        'Please enter a rejection reason before rejecting the request.'
      );

      return;

    }


    const reportId =
      this.selectedReport.id;


    const employeeName =
      this.selectedReport.employeeName;


    const url =
      `${this.apiUrl}/joining-reports/${reportId}/reject-level2`;


    // IMPORTANT:
    // This must match your C# Level2RejectRequest class.
    const requestBody = {

      managerId: this.managerId,

      reason: reason

    };


    console.log(
      'Rejecting Level 2 joining request:',
      reportId
    );


    console.log(
      'Rejection reason:',
      reason
    );


    console.log(
      'Reject URL:',
      url
    );


    console.log(
      'Reject body:',
      requestBody
    );


    this.actionLoading = true;

    this.errorMessage = '';

    this.successMessage = '';


    this.http

      .put<JoiningReport>(

        url,

        requestBody

      )

      .pipe(

        timeout(10000),

        finalize(() => {

          this.actionLoading = false;

          this.cdr.detectChanges();

        })

      )

      .subscribe({

        next: (response) => {

          console.log(
            'Level 2 rejection successful:',
            response
          );


          this.successMessage =
            `Joining request #${reportId} for ${employeeName} has been rejected.`;


          alert(
            `Joining request #${reportId} for ${employeeName} has been rejected.`
          );


          // Remove rejected request from pending list
          this.reports =
            this.reports.filter(
              report => report.id !== reportId
            );


          this.showReportModal = false;

          this.selectedReport = null;

          this.rejectionReason = '';


          this.cdr.detectChanges();

        },


        error: (error) => {

          console.error(
            'Level 2 rejection failed:',
            error
          );


          console.error(
            'Status:',
            error.status
          );


          console.error(
            'Response:',
            error.error
          );


          if (error.status === 400) {

            this.errorMessage =
              error.error ||
              'The server rejected the rejection request.';

          }

          else if (error.status === 404) {

            this.errorMessage =
              'Joining request was not found.';

          }

          else if (error.status === 0) {

            this.errorMessage =
              'Unable to connect to the backend server.';

          }

          else {

            this.errorMessage =
              'Failed to reject the joining request.';

          }


          alert(this.errorMessage);


          this.cdr.detectChanges();

        }

      });

  }
  // =====================================================
// APPROVAL STATUS
// =====================================================

getLevel1Status(report: JoiningReport): string {
  return 'Approved';
}

getLevel2Status(report: JoiningReport): string {
  return 'Pending';
}

getHrStatus(report: JoiningReport): string {
  return 'Pending';
}


  // =====================================================
  // TRACK BY
  // =====================================================

  trackByReportId(
    index: number,
    report: JoiningReport
  ): number {

    return report.id;

  }

}