import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RegisterComponent} from './register/register.component';
import {ForgotPasswordComponent} from './forgot-password/forgot-password.component';
import {ResetPasswordComponent} from './reset-password/reset-password.component';
import {LoginComponent} from './login/login.component';
import {ProfileComponent} from './profile/profile.component';
import {TwoFactorComponent} from './two-factor/two-factor.component';

const routes: Routes = [
	{
		path: 'register',
		component: RegisterComponent
	},
	{
		path: "forgot-password",
		component: ForgotPasswordComponent
	},
	{
		path: "reset-password",
		component: ResetPasswordComponent
	},
	{
		path: "two-factor",
		component: TwoFactorComponent
	},
	{
		path: "login",
		component: LoginComponent
	},
	{
		path: "profile",
		component: ProfileComponent
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class AccountRoutingModule { }
