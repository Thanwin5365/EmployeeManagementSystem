import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { environment } from '../../environments/environment';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manager-home',
  standalone: true,

  imports: [CommonModule, FormsModule],

  templateUrl: './manager-home.html',
  styleUrl: './manager-home.css',
})
export class ManagerHome implements OnInit {
  // API

  private readonly API_URL = environment.apiUrl;

  // MANAGER INFORMATION

  manager = {
    name: 'Suraj',
    managerId: 201,
    managerLevel: 'Level 1',
    managerKey: 'MGR-001',
  };

  // POPUPS

  showPendingRequests = false;
  showReport = false;

  // LOADING

  loading = false;
  actionLoading = false;

  
  // JOINING REPORTS
  

  pendingReports: any[] = [];
  selectedReport: any = null;

  
  // REJECTION REASON
  

  rejectionReason = '';

  
  // CONSTRUCTOR
  

  constructor(
    private http: HttpClient,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  
  // INITIALIZE
 

  ngOnInit(): void {
    const managerId = sessionStorage.getItem('managerId');

    // If manager is not logged in
    if (!managerId) {
      this.router.navigate(['/login']);

      return;
    }

    // Load manager name
    const managerName = sessionStorage.getItem('managerName');

    if (managerName) {
      this.manager.name = managerName;
    }

    // Load manager ID
    const storedManagerId = Number(managerId);

    if (!isNaN(storedManagerId)) {
      this.manager.managerId = storedManagerId;
    }

    // Load manager key
    const storedManagerKey = sessionStorage.getItem('managerKey');

    if (storedManagerKey) {
      this.manager.managerKey = storedManagerKey;
    }
  }

  
  // LOGOUT
 

  logout(): void {
    if (this.actionLoading) {
      return;
    }

    // Clear manager session
    sessionStorage.removeItem('managerId');
    sessionStorage.removeItem('managerName');
    sessionStorage.removeItem('managerKey');

    // Clear local data
    this.showPendingRequests = false;
    this.showReport = false;
    this.selectedReport = null;
    this.pendingReports = [];
    this.rejectionReason = '';

    // Navigate to login
    this.router.navigate(['/login']);
  }

  
  // OPEN PENDING REQUESTS
  

  openPendingRequests(): void {
    if (this.actionLoading) {
      return;
    }

    this.showPendingRequests = true;
    this.showReport = false;

    this.loading = true;
    this.pendingReports = [];

    console.log('Loading pending joining reports...');

    this.http.get<any[]>(`${this.API_URL}/joining-reports/pending`).subscribe({
      
      // SUCCESS
      

      next: (reports) => {
        console.log('Pending joining reports:', reports);

        this.pendingReports = reports || [];

        this.loading = false;

        this.cdr.detectChanges();
      },

     
      // ERROR
      

      error: (error) => {
        console.error('Unable to load pending joining reports:', error);

        this.loading = false;

        alert('Unable to load pending joining requests.');

        this.cdr.detectChanges();
      },
    });
  }

  
  // CLOSE PENDING REQUESTS
  

  closePendingRequests(): void {
    if (this.actionLoading) {
      return;
    }

    this.showPendingRequests = false;
    this.loading = false;
  }

  // VIEW JOINING REPORT
  

  viewReport(report: any): void {
    console.log('Selected joining report:', report);

    // Store selected report
    this.selectedReport = report;

    // Load existing rejection reason
    this.rejectionReason = report?.rejectionReason || '';

    // Close pending requests popup
    this.showPendingRequests = false;

    // Open report popup
    this.showReport = true;

    this.cdr.detectChanges();
  }

  
  // CLOSE JOINING REPORT
  

  closeReport(): void {
    if (this.actionLoading) {
      return;
    }

    this.showReport = false;

    this.selectedReport = null;

    this.rejectionReason = '';
  }

  // =====================================================
  // GET JOINING REPORT ID
  // =====================================================

  private getReportId(): number | null {
    if (!this.selectedReport) {
      return null;
    }

    /*
     * IMPORTANT:
     *
     * Use the joining report ID.
     *
     * Example:
     *
     * id = 10
     * employeeId = 105
     *
     * API requires:
     *
     * /joining-reports/10/reject
     *
     * NOT:
     *
     * /joining-reports/105/reject
     */

    const reportId = Number(this.selectedReport.id);

    if (!reportId || reportId <= 0) {
      console.error('Invalid joining report:', this.selectedReport);

      return null;
    }

    return reportId;
  }

  // =====================================================
  // APPROVE / ACCEPT JOINING REPORT
  // =====================================================

  approveReport(): void {
    if (!this.selectedReport) {
      alert('No joining report selected.');

      return;
    }

    if (this.actionLoading) {
      return;
    }

    // IMPORTANT:
    // Use joining report ID, NOT employee ID.
    const reportId = this.getReportId();

    if (!reportId) {
      alert('Joining report ID not found.');

      return;
    }

    // Get manager ID
    const managerId = Number(sessionStorage.getItem('managerId')) || this.manager.managerId;

    // Request body
    const request = {
      approvedBy: managerId,
    };

    console.log('================================');

    console.log('APPROVING JOINING REPORT');

    console.log('Report ID:', reportId);

    console.log('Employee ID:', this.selectedReport.employeeId);

    console.log('Request:', request);

    console.log('URL:', `${this.API_URL}/joining-reports/${reportId}/approve`);

    console.log('================================');

    this.actionLoading = true;

    this.http.put(`${this.API_URL}/joining-reports/${reportId}/approve`, request).subscribe({
      // =============================================
      // SUCCESS
      // =============================================

      next: (response) => {
        console.log('Joining report approved:', response);

        this.actionLoading = false;

        alert('Joining request accepted successfully.');

        // Close report
        this.showReport = false;

        this.selectedReport = null;

        this.rejectionReason = '';

        // Refresh pending requests
        this.openPendingRequests();

        this.cdr.detectChanges();
      },

      // =============================================
      // ERROR
      // =============================================

      error: (error) => {
        console.error('Unable to approve joining report:', error);

        console.error('Status:', error.status);

        console.error('URL:', error.url);

        console.error('Backend response:', error.error);

        this.actionLoading = false;

        alert(error?.error || 'Unable to approve joining report.');

        this.cdr.detectChanges();
      },
    });
  }

  // =====================================================
  // REJECT JOINING REPORT
  // =====================================================

  rejectReport(): void {
    if (!this.selectedReport) {
      alert('No joining report selected.');
      return;
    }

    if (this.actionLoading) {
      return;
    }

    const reason = (this.rejectionReason || '').trim();

    if (!reason) {
      alert('Please enter a rejection reason.');
      return;
    }

    // IMPORTANT:
    // Backend expects the report ID, not employee ID.
    const reportId = this.selectedReport.id ?? this.selectedReport.reportId;

    if (!reportId) {
      alert('Joining report ID not found.');
      console.error('Selected report:', this.selectedReport);
      return;
    }

    this.actionLoading = true;

    const url = `${this.API_URL}/joining-reports/${reportId}/reject`;

    console.log('================================');
    console.log('REJECTING JOINING REPORT');
    console.log('Report ID:', reportId);
    console.log('Employee ID:', this.selectedReport.employeeId);
    console.log('Employee Name:', this.selectedReport.employeeName);
    console.log('Rejection Reason:', reason);
    console.log('URL:', url);
    console.log('================================');

    /*
     * IMPORTANT:
     *
     * Your backend expects a STRING in the request body.
     *
     * Do NOT send:
     *
     * {
     *   rejectionReason: reason,
     *   rejectedBy: managerId
     * }
     *
     * Send the reason as a JSON string instead.
     */

    const body = JSON.stringify(reason);

    this.http
      .put(url, body, {
        headers: {
          'Content-Type': 'application/json',
        },
      })
      .subscribe({
        next: (response) => {
          console.log('REJECT SUCCESS:', response);

          this.actionLoading = false;

          alert('Joining request rejected successfully.');

          // Close report popup
          this.showReport = false;

          // Clear selected report
          this.selectedReport = null;

          // Clear rejection reason
          this.rejectionReason = '';

          // Refresh pending requests
          this.openPendingRequests();

          this.cdr.detectChanges();
        },

        error: (error) => {
          console.error('================================');
          console.error('REJECT API ERROR');
          console.error('Status:', error.status);
          console.error('URL:', error.url);
          console.error('Backend response:', error.error);
          console.error('Full error:', error);
          console.error('================================');

          this.actionLoading = false;

          let message = 'Unable to reject joining report.';

          if (typeof error.error === 'string') {
            message = error.error;
          } else if (error.error?.title) {
            message = error.error.title;
          } else if (error.error?.message) {
            message = error.error.message;
          }

          alert(message);

          this.cdr.detectChanges();
        },
      });
  }
}
