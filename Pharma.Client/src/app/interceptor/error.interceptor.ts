import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { PharmacyMerchantService } from '../service/pharmacy-merchant.service';
import { catchError, throwError } from 'rxjs';
import { ToastMessageService } from '../service/toast-message.service';
import { authCookieKey } from '../common/constant/auth-key';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(PharmacyMerchantService);
  const toastService = inject(ToastMessageService);
  const router = inject(Router);
  return next(req).pipe(
    catchError((err: any) => {
      console.log(err);
      if ([401].includes(err.status) && localStorage.getItem(authCookieKey)) {
        toastService.showError("Unauthorized", "Session Expired!");
        accountService.logOut();
        router.navigate(['/pharmacy-login']);
      }
      const error = err.statusText;
      return throwError(() => error);
    })
  );
};
