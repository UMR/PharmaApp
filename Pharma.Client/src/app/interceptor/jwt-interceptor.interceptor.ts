import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { PharmacyMerchantService } from '../service/pharmacy-merchant.service';

export const jwtInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const pharmacyMerchantService = inject(PharmacyMerchantService);
  const user = pharmacyMerchantService.userValue;
  const isLoggedIn = user && user?.token;
  if (isLoggedIn) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${user.token}`
      }
    });
  }

  return next(req);
};
