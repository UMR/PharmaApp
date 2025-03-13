import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AngularPhoneNumberInput } from 'angular-phone-number-input';



@NgModule({
  declarations: [],
  imports: [
    AngularPhoneNumberInput,
    CommonModule
  ],
  exports: [
    AngularPhoneNumberInput
  ]
})
export class OtherModule { }
