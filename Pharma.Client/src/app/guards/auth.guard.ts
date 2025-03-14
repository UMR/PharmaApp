import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { PharmacyMerchantService } from '../service/pharmacy-merchant.service';
import { authCookieKey } from '../common/constant/auth-key';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private pharmacyService: PharmacyMerchantService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const user = localStorage.getItem(authCookieKey);
    if (user) {
      return true;
    }

    this.router.navigate(['/pharmacy-login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}