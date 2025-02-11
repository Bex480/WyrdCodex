import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ApiConfig } from '../../config/api.config';

@Component({
	selector: 'app-forgot-password',
	standalone: false,
	templateUrl: './forgot-password.component.html',
	styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
	forgotPasswordForm: FormGroup;
	private email: any;
	success: boolean = false;

	constructor(private fb: FormBuilder, private http: HttpClient) {
		this.forgotPasswordForm = this.fb.group({
			email: ['', [Validators.required, Validators.email]]
		});
	}

	isInvalid(fieldName: string): boolean {
		const control = this.forgotPasswordForm.get(fieldName);
		return !!(control?.invalid && (control.dirty || control.touched));
	}


	onSubmit() {
		if (this.forgotPasswordForm.valid) {
			this.email = this.forgotPasswordForm.value.email;
			this.http.post(`${ApiConfig.apiUrl}/User/request_password_reset?email=${this.email}`, null).subscribe();
			this.success = true;
		} else {
			this.forgotPasswordForm.markAllAsTouched();
		}
	}
}
