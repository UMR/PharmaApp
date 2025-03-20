import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { authCookieKey } from '../common/constant/auth-key';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    debugger;
    const user = localStorage.getItem(authCookieKey) ? JSON.parse(localStorage.getItem(authCookieKey)!) : null;
    if (user && user.accessToken != null) {
      return true;
    }

    this.router.navigate(['/pharmacy-login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}