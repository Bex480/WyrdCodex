import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule } from "@angular/forms";
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
  declarations: [
    RegisterComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    LoginComponent,
    ProfileComponent
  ],
    imports: [
        CommonModule,
        AccountRoutingModule,
        ReactiveFormsModule
    ]
})
export class AccountModule { }
