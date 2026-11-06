import { Routes } from '@angular/router';
import { authGuard } from './auth-guard';
import { Level2ManagerHome } from './level2-manager-home/level2-manager-home';
import { Login } from './pages/login/login';
import { EmployeeHome } from './pages/employee-home/employee-home';
import { ManagerHome } from './manager-home/manager-home';
import { HrHome } from './pages/hr-home/hr-home';
import { Dashboard } from './pages/dashboard/dashboard';


export const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: Login
  },

  {
    path: 'employee-home',
    component: EmployeeHome
  },

  {
    path: 'manager-home',
    component: ManagerHome
  },

  {
    path: 'level2-manager-home',
    component: Level2ManagerHome
  },

  {
    path: 'hr-home',
    component: HrHome
  },

  {
  path: 'dashboard',
  component: Dashboard,
  canActivate: [authGuard]
},

];