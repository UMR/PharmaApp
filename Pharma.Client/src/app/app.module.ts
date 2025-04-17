import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { providePrimeNG } from 'primeng/config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BasicInfoRegistrationPageComponent } from './pages/basic-info-registration-page/basic-info-registration-page.component';
import { BinahScanComponent } from './pages/binah-scan/binah-scan.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PrimengModule } from './common/primeng/primeng.module';
import { WasmService } from './service/wasm.service';
import { LoginComponent } from './pages/pharmacist-registration-login-panel/login/login.component';
import { RegistrationComponent } from './pages/pharmacist-registration-login-panel/registration/registration.component';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { OtpComponent } from './pages/pharmacist-registration-login-panel/otp/otp.component';
import { MessageService } from 'primeng/api';
import { OtherModule } from './common/other/other.module';
import { PharmacyDashboardLayoutComponent } from './pages/pharmacist-dashboard-panel/pharmacy-dashboard-layout/pharmacy-dashboard-layout.component';
import { HeaderComponent } from './common/component/header/header.component';
import { SidebarComponent } from './common/component/sidebar/sidebar.component';
import { PharmacyQrComponent } from './pages/pharmacist-dashboard-panel/pharmacy-qr/pharmacy-qr.component';
import { jwtInterceptor } from './interceptor/jwt-interceptor.interceptor';
import { errorInterceptor } from './interceptor/error.interceptor';
import { PayNowComponent } from './pages/customer-panel/pay-now/pay-now.component';
import { ScanLogComponent } from './pages/pharmacist-dashboard-panel/scan-log/scan-log.component';
import { DashboardComponent } from './pages/pharmacist-dashboard-panel/dashboard/dashboard.component';
import { LedgerComponent } from './pages/pharmacist-dashboard-panel/ledger/ledger.component';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard/admin-dashboard.component';
import { UserManagementComponent } from './pages/admin/user-management/user-management.component';

@NgModule({
  declarations: [
    AppComponent,
    BasicInfoRegistrationPageComponent,
    BinahScanComponent,
    LoginComponent,
    RegistrationComponent,
    OtpComponent,
    PharmacyDashboardLayoutComponent,
    HeaderComponent,
    SidebarComponent,
    PharmacyQrComponent,
    PayNowComponent,
    ScanLogComponent,
    DashboardComponent,
    LedgerComponent,
    AdminDashboardComponent,
    UserManagementComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    PrimengModule,
    FormsModule,
    OtherModule,

  ],
  providers: [
    provideAnimationsAsync(),
    providePrimeNG(),
    WasmService,
    MessageService,
    provideHttpClient(
      withInterceptorsFromDi(),
      withInterceptors([jwtInterceptor, errorInterceptor])
    ),

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }