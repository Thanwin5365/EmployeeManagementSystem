import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../../services/employee.service';
import { Router } from '@angular/router';
import { AttendanceService, AttendanceResponse } from '../../services/attendance.service';

@Component({
  selector: 'app-employee-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-home.html',
  styleUrl: './employee-home.css',
})
export class EmployeeHome implements OnInit {

  // =====================================================
  // EMPLOYEE INFORMATION
  // =====================================================

  employee: any = {
    employeeId: '',
    name: '',
    designation: '',
    department: '',
    email: '',
    phone: '',
    location: '',
  };


  // =====================================================
  // JOINING FORM
  // =====================================================

  showJoiningForm = false;

  formStatus: string = 'Not Submitted';

  approvalStatus: string = 'Pending';

  rejectionReason: string = '';


  // =====================================================
  // PREVIOUS JOINING REPORTS
  // =====================================================

  joiningReports: any[] = [];

  totalReports = 0;

  acceptedReports = 0;

  rejectedReports = 0;

  pendingReports = 0;

  selectedReport: any = null;

  showPreviousReport = false;


  // =====================================================
  // JOINING FORM DATA
  // =====================================================

  joiningForm = {
    employeeName: '',
    designation: '',
    department: '',
    joinedDutyOn: '',
    joiningType: 'After Vacation',
    leaveAppliedTill: '',
    delayReason: '',
    date: '',
    signature: '',
  };


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private router: Router,
    private employeeService: EmployeeService,
    private cdr: ChangeDetectorRef,
  ) {
    this.loadSavedForm();
  }


  // =====================================================
  // PAGE LOAD
  // =====================================================

  ngOnInit(): void {

    const employeeId =
      sessionStorage.getItem('employeeId');

    if (!employeeId) {

      this.router.navigate(['/login']);

      return;
    }

    console.log(
      'Loading employee:',
      employeeId
    );

    


    // ===================================================
    // GET EMPLOYEE INFORMATION
    // ===================================================

    this.employeeService
      .getEmployeeById(Number(employeeId))
      .subscribe({

        next: (response: any) => {

          console.log(
            'Employee details:',
            response
          );


          this.employee = {

            employeeId:
              response.employeeId,

            name:
              response.name,

            designation:
              response.designation,

            department:
              response.department,

            email:
              response.email,

            phone:
              response.phone,

            location:
              response.location,

          };


          // Load joining reports
          this.loadJoiningReport();


          this.cdr.detectChanges();

        },


        error: (error: any) => {

          console.error(
            'Unable to load employee:',
            error
          );


          alert(
            'Unable to load employee details.'
          );

        },

      });

  }


  // =====================================================
  // LOAD JOINING REPORTS
  // =====================================================

  loadJoiningReport(): void {

    const employeeId =
      sessionStorage.getItem('employeeId');


    if (!employeeId) {

      return;

    }


    console.log(
      'Loading joining reports for employee:',
      employeeId
    );

    // Business logic is handled by EmployeeService
    this.employeeService
      .getJoiningReportSummary(
        Number(employeeId)
      )
      .subscribe({

        next: (result: any) => {
        

          console.log(
            'Joining report summary:',
            result
          );


          // ==============================================
          // ALL REPORTS
          // ==============================================

          this.joiningReports =
            result.reports || [];


          // ==============================================
          // REPORT SUMMARY
          // ==============================================

          this.totalReports =
            result.total || 0;


          this.acceptedReports =
            result.accepted || 0;


          this.rejectedReports =
            result.rejected || 0;


          this.pendingReports =
            result.pending || 0;


          console.log(
            'Total reports:',
            this.totalReports
          );


          console.log(
            'Accepted:',
            this.acceptedReports
          );
          

          console.log(
            'Rejected:',
            this.rejectedReports
          );


          console.log(
            'Pending:',
            this.pendingReports
          );


          // ==============================================
          // NO REPORT
          // ==============================================

          if (
            this.joiningReports.length === 0
          ) {

            this.formStatus =
              'Not Submitted';


            this.approvalStatus =
              'Pending';


            this.rejectionReason =
              '';


            this.cdr.detectChanges();

            return;

          }


          // ==============================================
          // GET LATEST REPORT
          // ==============================================

          let report =
            this.joiningReports[0];


          /*
           * If backend returns multiple reports,
           * use the report with the highest ID.
           */

          if (
            this.joiningReports.length > 1
          ) {

            report =
              this.joiningReports.reduce(
                (latest, current) => {

                  return Number(
                    current.id || 0
                  ) >
                  Number(
                    latest.id || 0
                  )
                    ? current
                    : latest;

                },
                this.joiningReports[0]
              );

          }


          console.log(
            'Latest joining report:',
            report
          );


          // ==============================================
          // LOAD LATEST FORM DATA
          // ==============================================

          this.joiningForm = {

            employeeName:
              report.employeeName ||
              this.employee.name,


            designation:
              report.designation ||
              this.employee.designation,


            department:
              report.department ||
              this.employee.department,


            joinedDutyOn:
              report.joinedDutyOn ||
              '',


            joiningType:
              report.joiningType ||
              'After Vacation',


            leaveAppliedTill:
              report.leaveAppliedTill ||
              '',


            delayReason:
              report.delayReason ||
              '',


            date:
              report.formDate ||
              '',


            signature:
              report.signature ||
              '',

          };


          // ==============================================
          // FORM STATUS
          // ==============================================

          this.formStatus =
            'Submitted';


          // ==============================================
          // APPROVAL STATUS
          // ==============================================

          this.approvalStatus =
            report.approvalStatus ||
            'Pending';


          // ==============================================
          // REJECTION REASON
          // ==============================================

          this.rejectionReason =
            report.rejectionReason ||
            '';


          console.log(
            'Form status:',
            this.formStatus
          );


          console.log(
            'Approval status:',
            this.approvalStatus
          );


          console.log(
            'Rejection reason:',
            this.rejectionReason
          );


          this.cdr.detectChanges();

        },


        error: (error) => {

          console.error(
            'Unable to load joining reports:',
            error
          );

        }

      });

  }


  // =====================================================
  // VIEW PREVIOUS JOINING REPORT
  // =====================================================

  viewPreviousReport(
    report: any
  ): void {

    this.selectedReport =
      report;

    this.showPreviousReport =
      true;

  }


  // =====================================================
  // CLOSE PREVIOUS JOINING REPORT
  // =====================================================

  closePreviousReport(): void {

    this.selectedReport =
      null;

    this.showPreviousReport =
      false;

  }


  // =====================================================
  // OPEN JOINING FORM
  // =====================================================

  openJoiningForm(): void {

    this.loadSavedForm();


    this.joiningForm.employeeName =
      this.joiningForm.employeeName ||
      this.employee.name;


    this.joiningForm.designation =
      this.joiningForm.designation ||
      this.employee.designation;


    this.joiningForm.department =
      this.joiningForm.department ||
      this.employee.department;


    this.showJoiningForm =
      true;

  }


  // =====================================================
  // CLOSE JOINING FORM
  // =====================================================

  closeJoiningForm(): void {

    this.showJoiningForm =
      false;

  }


  // =====================================================
  // SAVE DRAFT
  // =====================================================

  saveDraft(): void {

    localStorage.setItem(

      `joiningForm_${this.employee.employeeId}`,

      JSON.stringify(
        this.joiningForm
      ),

    );


    localStorage.setItem(

      `joiningFormStatus_${this.employee.employeeId}`,

      'Draft Saved',

    );


    this.formStatus =
      'Draft Saved';


    alert(
      'Joining form saved as draft.'
    );


    this.showJoiningForm =
      false;

  }

  


  // =====================================================
  // CLEAR FORM
  // =====================================================

  clearForm(): void {

    this.joiningForm = {

      employeeName:
        this.employee.name,


      designation:
        this.employee.designation,


      department:
        this.employee.department,


      joinedDutyOn:
        '',


      joiningType:
        'After Vacation',


      leaveAppliedTill:
        '',


      delayReason:
        '',


      date:
        '',


      signature:
        '',

    };


    localStorage.removeItem(
      `joiningForm_${this.employee.employeeId}`
    );


    localStorage.removeItem(
      `joiningFormStatus_${this.employee.employeeId}`
    );


    this.formStatus =
      'Not Submitted';


    this.approvalStatus =
      'Pending';


    this.rejectionReason =
      '';

  }


  // =====================================================
  // SAVE / SUBMIT JOINING FORM
  // =====================================================

  saveJoiningForm(): void {

    if (!this.validateForm()) {

      return;

    }

    


    const employeeId =
      sessionStorage.getItem('employeeId');


    if (!employeeId) {

      alert(
        'Employee ID not found.'
      );

      return;

    }
    
    const formDate = {
      currentDate: new Date().toDateString(),
      currentTime: new Date().toLocaleDateString(),
      incrementedDate: new Date(
        new Date().setDate(
          new Date().getDate() 
        
        )
      )
    }

    const joiningReport = {

      employeeId:
        Number(employeeId),


      employeeName:
        this.joiningForm.employeeName,


      designation:
        this.joiningForm.designation,


      department:
        this.joiningForm.department,


      joinedDutyOn:
        this.joiningForm.joinedDutyOn,


      joiningType:
        this.joiningForm.joiningType,


      leaveAppliedTill:
        this.joiningForm.leaveAppliedTill ||
        null,


      delayReason:
        this.joiningForm.delayReason ||
        null,


      formDate:
        this.joiningForm.date,


      signature:
        this.joiningForm.signature,

    };


    console.log(
      'Submitting joining report:',
      joiningReport
    );


    this.employeeService
      .createJoiningReport(
        joiningReport
      )
      .subscribe({

        // ==============================================
        // SUCCESS
        // ==============================================

        next: (response: any) => {

          console.log(
            'Joining report saved:',
            response
          );


          this.formStatus =
            'Submitted';


          this.approvalStatus =
            'Pending';


          this.rejectionReason =
            '';


          alert(
            'Joining form submitted successfully.'
          );


          this.showJoiningForm =
            false;


          // Reload from backend
          this.loadJoiningReport();


          this.cdr.detectChanges();

        },


        // ==============================================
        // ERROR
        // ==============================================

        error: (error: any) => {

          console.error(
            'Unable to save joining report:',
            error
          );


          alert(
            'Unable to save joining form.'
          );

        }

      });

  }


  // =====================================================
  // VALIDATION
  // =====================================================

  validateForm(): boolean {

    if (
      !this.joiningForm.employeeName.trim()
    ) {

      alert(
        'Please enter employee name.'
      );

      return false;

    }


    if (
      !this.joiningForm.designation.trim()
    ) {

      alert(
        'Please enter designation.'
      );

      return false;

    }


    if (
      !this.joiningForm.department.trim()
    ) {

      alert(
        'Please enter department.'
      );

      return false;

    }


    if (
      !this.joiningForm.joinedDutyOn
    ) {

      alert(
        'Please select joined duty date.'
      );

      return false;

    }


    if (
      !this.joiningForm.date
    ) {

      alert(
        'Please select the form date.'
      );

      return false;

    }


    if (
      !this.joiningForm.signature.trim()
    ) {

      alert(
        'Please enter employee signature.'
      );

      return false;

    }


    return true;

  }


  // =====================================================
  // LOAD SAVED FORM
  // =====================================================

  loadSavedForm(): void {

    if (
      !this.employee.employeeId
    ) {

      return;

    }


    const savedForm =
      localStorage.getItem(
        `joiningForm_${this.employee.employeeId}`
      );


    const savedStatus =
      localStorage.getItem(
        `joiningFormStatus_${this.employee.employeeId}`
      );


    if (savedForm) {

      try {

        this.joiningForm =
          JSON.parse(savedForm);

      } catch (error) {

        console.error(
          'Unable to load saved joining form:',
          error
        );

      }

    }


    if (savedStatus) {

      this.formStatus =
        savedStatus;

    }

  }


  // =====================================================
  // LOGOUT
  // =====================================================

  logout(): void {

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

}