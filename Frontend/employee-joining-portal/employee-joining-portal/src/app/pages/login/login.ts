import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  loginId: string = '';
  password: string = '';

  errorMessage: string = '';
  loading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  login(): void {

    this.errorMessage = '';

    // VALIDATION

    if (!this.loginId.trim() || !this.password.trim()) {

      this.errorMessage =
        'Please enter Username and password.';

      return;
    }

    this.loading = true;

    // TRY EMPLOYEE LOGIN FIRST

    this.authService
      .employeeLogin(
        this.loginId,
        this.password
      )
      .subscribe({

        next: (response) => {

          console.log(
            'Employee login successful:',
            response
          );

          // Clear old manager / HR information

          sessionStorage.removeItem('managerId');
          sessionStorage.removeItem('managerName');
          sessionStorage.removeItem('managerKey');
          sessionStorage.removeItem('managerLevel');
          sessionStorage.removeItem('hrManagerId');
          sessionStorage.removeItem('hrManagerName');
          

          // Save employee information

          sessionStorage.setItem(
            'employeeId',
            response.employeeId.toString()
          );

        

          sessionStorage.setItem(
            'employeeName',
            response.name
          );

          sessionStorage.setItem(
            'role',
            'employee'
          );

          this.loading = false;

          this.router.navigate([
            '/dashboard'
          ]);
        },

        error: (employeeError) => {

          console.log(
            'Employee login failed. Trying manager login...'
          );

          // If employee login fails,
          // try manager login.

          this.authService
            .managerLogin(
              this.loginId,
              this.password
            )
            .subscribe({

              next: (response) => {

                console.log(
                  'Manager / HR login successful:',
                  response
                );

                // Save manager information

                if (response.managerId != null) {

                  sessionStorage.setItem(
                    'managerId',
                    response.managerId.toString()
                  );

                }

                if (response.name) {

                  sessionStorage.setItem(
                    'managerName',
                    response.name
                  );

                }

                if (response.managerKey) {

                  sessionStorage.setItem(
                    'managerKey',
                    response.managerKey
                  );

                }

                if (response.managerLevel != null) {

                  sessionStorage.setItem(
                    'managerLevel',
                    response.managerLevel.toString()
                  );

                }

                // All manager levels use manager role

                sessionStorage.setItem(
                  'role',
                  'manager'
                );

                this.loading = false;

                this.router.navigate([
                  '/dashboard'
                ]);
              },

              error: (managerError) => {

                console.error(
                  'Manager login error:',
                  managerError
                );

                if (
                  employeeError.status === 401 &&
                  managerError.status === 401
                ) {

                  this.errorMessage =
                    'Invalid Username or password.';

                } else if (
                  employeeError.status === 0 ||
                  managerError.status === 0
                ) {

                  this.errorMessage =
                    'Unable to connect to the server.';

                } else {

                  this.errorMessage =
                    managerError.error?.message ||
                    'Something went wrong. Please try again.';
                }

                this.loading = false;
              }

            });
        }

      });
  }
}