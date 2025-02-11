import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { RegisterComponent } from './register/register.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { TwoFactorComponent } from './two-factor/two-factor.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import {NgOtpInputComponent} from 'ng-otp-input';
import {SharedModule} from '../shared/shared.module';

@NgModule({
  declarations: [
	  RegisterComponent,
	  ForgotPasswordComponent,
	  ResetPasswordComponent,
	  TwoFactorComponent,
	  LoginComponent,
	  ProfileComponent
  ],
	imports: [
		CommonModule,
		AccountRoutingModule,
		ReactiveFormsModule,
		FormsModule,
		NgOtpInputComponent,
		SharedModule
	]
})
export class AccountModule { }
