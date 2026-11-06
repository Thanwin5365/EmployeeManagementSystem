
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  
  const role = (sessionStorage.getItem('role') || '').toLowerCase();

  const employeeId = sessionStorage.getItem('employeeId');
  const managerId = sessionStorage.getItem('managerId');
  const managerLevel = Number(sessionStorage.getItem('managerLevel'));

  
  const isEmployee =
    role === 'employee' &&
    employeeId !== null &&
    Number(employeeId) > 0;

  
  const isManager =
    role === 'manager' &&
    managerId !== null &&
    Number(managerId) > 0 &&
    [1, 2, 3].includes(managerLevel);

  
  if (isEmployee || isManager) {
    return true;
  }

  
  return router.createUrlTree(['/login']);
};
