import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { AuthenticationService } from '../service/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class PharmacyResolver implements Resolve<any> {
  constructor(private pharmacyService: AuthenticationService, private router: Router) { }

  resolve(route: ActivatedRouteSnapshot) {
    const pharmacyId = route.queryParams['pharmacy'];
    return this.pharmacyService.getPharmacyByUniqueId(pharmacyId).pipe(
      map((res) => {
        if (res) {
          this.pharmacyService.setPharmacyData(res);
          return res;
        }
        this.router.navigate(['/pharmacy-login']);
        return null;
      }),
      catchError(() => {
        this.router.navigate(['/pharmacy-login']);
        return of(null);
      })
    );
  }
}
