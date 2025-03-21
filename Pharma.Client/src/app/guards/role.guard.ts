import { CanActivateFn, Router } from '@angular/router';
import { RoleType } from '../common/model/role.model';
import { AuthenticationService } from '../service/authentication.service';
import { inject } from '@angular/core';
import { ToastMessageService } from '../service/toast-message.service';

export const roleGuard: CanActivateFn = (route, state) => {
  debugger;
  const authService = inject(AuthenticationService);
  const toastService = inject(ToastMessageService);
  const requiredRoles = route.data['roles'] as RoleType;

  if (authService.hasRole(requiredRoles)) {
    return true;
  }
  toastService.showError("Unauthorized", "You don't have permission to access this page!");
  return false;
};
