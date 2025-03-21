import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasicInfoRegistrationPageComponent } from './pages/basic-info-registration-page/basic-info-registration-page.component';
import { BinahScanComponent } from './pages/binah-scan/binah-scan.component';
import { RegistrationComponent } from './pages/pharmacist-registration-login-panel/registration/registration.component';
import { LoginComponent } from './pages/pharmacist-registration-login-panel/login/login.component';
import { OtpComponent } from './pages/pharmacist-registration-login-panel/otp/otp.component';
import { PharmacyDashboardLayoutComponent } from './pages/pharmacist-dashboard-panel/pharmacy-dashboard-layout/pharmacy-dashboard-layout.component';
import { AuthGuard } from './guards/auth.guard';
import { PharmacyQrComponent } from './pages/pharmacist-dashboard-panel/pharmacy-qr/pharmacy-qr.component';
import { pharmacyScanGuard } from './guards/pharmacy-scan.guard';
import { PharmacyResolver } from './resolver/pharmacy.resolver';
import { PayNowComponent } from './pages/customer-panel/pay-now/pay-now.component';
import { RoleType } from './common/model/role.model';
import { roleGuard } from './guards/role.guard';

const routes: Routes = [
  {
    path: '',
    component: PharmacyDashboardLayoutComponent,
    children: [
      {
        path: '',
        component: BinahScanComponent,
        // canActivate: [pharmacyScanGuard]
      },
      {
        path: 'pharmacy-qr',
        component: PharmacyQrComponent,
        canActivate: [AuthGuard, roleGuard],
        data: {
          roles: [RoleType.PHARMACIST]
        }

      }
    ]
  },
  {
    path: 'otp-verification',
    component: OtpComponent,
  },
  {
    path: 'vital-scan',
    component: BinahScanComponent,
    canActivate: [AuthGuard, roleGuard],
    data: {
      roles: [RoleType.PHARMACIST]
    }
  },
  {
    path: 'pharmacy-registration',
    component: RegistrationComponent
  },
  {
    path: 'pharmacy-login',
    component: LoginComponent
  },
  {
    path: 'scan',
    component: BasicInfoRegistrationPageComponent,
    canActivate: [pharmacyScanGuard],
    resolve: {
      pharmacy: PharmacyResolver
    }
  },
  {
    path: 'pay-now',
    component: PayNowComponent,

  },

  {
    path: "**",
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [PharmacyResolver]
})
export class AppRoutingModule { }
