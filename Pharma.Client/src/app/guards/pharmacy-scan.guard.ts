import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { inject } from '@angular/core';

export const pharmacyScanGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  debugger;
  const router = inject(Router);
  const pharmacyId = route.queryParams['pharmacy'];

  if (!pharmacyId) {
    router.navigate(['/pharmacy-login']);
    return false;
  }
  return true;
};
