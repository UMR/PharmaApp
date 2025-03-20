import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthenticationService } from '../service/authentication.service';
import { inject } from '@angular/core';

export const pharmacyScanGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const pharmacyService = inject(AuthenticationService);
  const router = inject(Router);

  const pharmacyId = route.queryParams['pharmacy'];

  pharmacyService.getPharmacyByUniqueId(pharmacyId).subscribe({
    next: (res: any) => {
      if (res.body) {
        return true;
      }
      return false;
    },
    error: (err) => {
      router.navigate(['/pharmacy-login']);
      return false;
    }
  });
  return true;
};
