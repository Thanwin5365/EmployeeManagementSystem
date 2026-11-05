
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttendanceService, AttendanceResponse } from '../services/attendance.service';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './attendance.html',
  styleUrl: './attendance.css'
})
export class Attendance implements OnInit {

  employeeId!: number;

  attendance: AttendanceResponse | null = null;

  loading = false;
  processing = false;
  errorMessage = '';
  successMessage = '';

  constructor(private attendanceService: AttendanceService) {}

  ngOnInit(): void {
    const storedEmployeeId = sessionStorage.getItem('employeeId');

    if (!storedEmployeeId || Number(storedEmployeeId) <= 0) {
      this.errorMessage = 'Employee information not found. Please log in again.';
      return;
    }

    this.employeeId = Number(storedEmployeeId);
    this.loadAttendance();
  }

  loadAttendance(): void {
    this.loading = true;
    this.errorMessage = '';

    this.attendanceService.getTodayAttendance(this.employeeId).subscribe({
      next: (response) => {
        this.attendance = response;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage =
          error.error?.message || 'Unable to load attendance.';
      }
    });
  }

  get isCheckedIn(): boolean {
    return this.attendance?.status === "Didn't Punch Out";
  }

  checkIn(): void {
    this.processing = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.attendanceService.checkIn(this.employeeId).subscribe({
      next: (response) => {
        this.attendance = response.attendance;
        this.successMessage = response.message;
        this.processing = false;
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Check-in failed.';
        this.processing = false;
        this.loadAttendance();
      }
    });
  }

  checkOut(): void {
    this.processing = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.attendanceService.checkOut(this.employeeId).subscribe({
      next: (response) => {
        this.attendance = response.attendance;
        this.successMessage = response.message;
        this.processing = false;
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Check-out failed.';
        this.processing = false;
        this.loadAttendance();
      }
    });
  }
}
