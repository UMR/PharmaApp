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
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { OtpComponent } from './pages/pharmacist-registration-login-panel/otp/otp.component';
import { MessageService } from 'primeng/api';
import { jwtInterceptorInterceptor } from './interceptor/jwt-interceptor.interceptor';
import { OtherModule } from './common/other/other.module';
import { PharmacyDashboardLayoutComponent } from './pages/pharmacist-dashboard-panel/pharmacy-dashboard-layout/pharmacy-dashboard-layout.component';
import { HeaderComponent } from './common/component/header/header.component';
import { SidebarComponent } from './common/component/sidebar/sidebar.component';
import { PharmacyQrComponent } from './pages/pharmacist-dashboard-panel/pharmacy-qr/pharmacy-qr.component';

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
    PharmacyQrComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    PrimengModule,
    FormsModule,
    OtherModule

  ],
  providers: [
    provideAnimationsAsync(),
    providePrimeNG(),
    WasmService,
    MessageService,
    provideHttpClient(),
    {
      provide: HTTP_INTERCEPTORS, useFactory: () => jwtInterceptorInterceptor, multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }