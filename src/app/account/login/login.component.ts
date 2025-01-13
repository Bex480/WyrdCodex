import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ApiConfig } from '../../config/api.config';

@Component({
	selector: 'app-login',
	standalone: false,
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent {
	loginForm: FormGroup;
	isFormSubmitted: boolean = false;
	loading: boolean = false;

	constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
		this.loginForm = this.fb.group({
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required, Validators.minLength(6)]]
		});
	}

	onSubmit() {
		this.isFormSubmitted = true;

		if (this.loginForm.valid) {
			this.loading = true;

			const formData = {
				email: this.loginForm.value.email,
				password: this.loginForm.value.password
			};

			this.http.post(`${ApiConfig.apiUrl}/User/login`, formData, { observe: 'response' })
				.subscribe(
					(response: any) => {
						if (response.status === 202) {
							this.router.navigate(['/account/two-factor'], { queryParams: { email: formData.email } });
						} else if (response.status === 200) {
							const token = response.body.token;
							const refToken = response.body.refreshToken;
							localStorage.setItem('authToken', token);
							localStorage.setItem('refToken', refToken)

							this.router.navigate(['/dashboard']);
						}
					},
					(error) => {
						console.error('Login failed:', error);
						this.router.navigate(['/account/login']);
					}
				);
		}
	}

	isInvalid(controlName: string): boolean {
		const control = this.loginForm.get(controlName);
		return !!(control && this.isFormSubmitted && !control.valid);
	}

	get email() {
		return this.loginForm.get('email');
	}

	get password() {
		return this.loginForm.get('password');
	}
}
