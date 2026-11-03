import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeHome } from '../employee-home/employee-home';
import { HrHome } from '../hr-home/hr-home';

import { ManagerHome } from '../../manager-home/manager-home';
import { Level2ManagerHome } from '../../level2-manager-home/level2-manager-home';

@Component({
  selector: 'app-dashboard',
  standalone: true,

  imports: [
    CommonModule,
    EmployeeHome,
    ManagerHome,
    Level2ManagerHome,
    HrHome
  ],

  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  role: string = '';
  managerLevel: number = 0;

  constructor() {

    this.role =
      sessionStorage.getItem('role') || '';

    this.managerLevel =
      Number(
        sessionStorage.getItem('managerLevel')
      ) || 0;

      this.role = this.role.toLocaleLowerCase(); 

  }

}