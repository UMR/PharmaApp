import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { PharmacyMerchantService } from '../service/pharmacy-merchant.service';
import { environment } from '../../environments/environment';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(PharmacyMerchantService);
  const user = accountService.userValue;
  const isLoggedIn = user && user.accessToken;
  const isApiUrl = req.url.startsWith(environment.apiUrl);

  if (isLoggedIn && isApiUrl) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${user.accessToken}`
      }
    });
  }

  return next(req);
};
