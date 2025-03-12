import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasicInfoRegistrationPageComponent } from './pages/basic-info-registration-page/basic-info-registration-page.component';
import { BinahScanComponent } from './pages/binah-scan/binah-scan.component';
import { RegistrationComponent } from './pages/pharmacist-registration-login-panel/registration/registration.component';
import { LoginComponent } from './pages/pharmacist-registration-login-panel/login/login.component';
import { OtpComponent } from './pages/pharmacist-registration-login-panel/otp/otp.component';

const routes: Routes = [
  {
    path: 'patient-registration',
    component: BasicInfoRegistrationPageComponent
  },
  {
    path: 'otp-verification',
    component: OtpComponent,
  },
  {
    path: 'vital-scan',
    component: BinahScanComponent
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
    path: "**",
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
