import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ApiConfig } from '../../config/api.config';

@Component({
	selector: 'app-reset-password',
	standalone: false,
	templateUrl: './reset-password.component.html',
	styleUrls: ['./reset-password.component.css']
})

export class ResetPasswordComponent implements OnInit {
	resetPasswordForm: FormGroup;
	token: string | null = null;

	constructor(
		private fb: FormBuilder,
		private http: HttpClient,
		private route: ActivatedRoute,
		private router: Router
	) {
		this.resetPasswordForm = this.fb.group(
			{
				password: ['', [Validators.required, Validators.minLength(6)]],
				repeatPassword: ['', Validators.required]
			},
			{
				validator: this.passwordMatchValidator
			}
		);
	}

	ngOnInit(): void {
		this.token = this.route.snapshot.queryParamMap.get('token');

		if (!this.token) {
			console.error('Token is missing.');
			this.router.navigate(['/']); // Redirect to home or login page if token is missing
		}
	}

	passwordMatchValidator(form: FormGroup) {
		const password = form.get('password')?.value;
		const repeatPassword = form.get('repeatPassword')?.value;

		return password && repeatPassword && password === repeatPassword ? null : { mismatch: true };
	}

	onSubmit(): void {
		if (this.resetPasswordForm.valid && this.token) {
			const { password } = this.resetPasswordForm.value;

			this.http
				.put(`${ApiConfig.apiUrl}/User/reset_password`, { token: this.token, newPassword: password })
				.subscribe();
		}
	}
}
