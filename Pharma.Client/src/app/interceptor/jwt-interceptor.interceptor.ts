import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { authCookieKey } from '../common/constant/auth-key';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const user = JSON.parse(localStorage.getItem(authCookieKey)!) || null;
  const isLoggedIn = user && user.accessToken != null;
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
