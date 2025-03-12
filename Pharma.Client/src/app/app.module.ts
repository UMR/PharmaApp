import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { providePrimeNG } from 'primeng/config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BasicInfoRegistrationPageComponent } from './pages/basic-info-registration-page/basic-info-registration-page.component';
import { BinahScanComponent } from './pages/binah-scan/binah-scan.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PrimengModule } from './common/primeng/primeng.module';
import { WasmService } from './service/wasm.service';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';

@NgModule({
  declarations: [
    AppComponent,
    BasicInfoRegistrationPageComponent,
    BinahScanComponent,
    LandingPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    PrimengModule

  ],
  providers: [
    provideAnimationsAsync(),
    providePrimeNG(),
    WasmService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
