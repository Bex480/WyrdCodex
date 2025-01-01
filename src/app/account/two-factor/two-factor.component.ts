import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {ApiConfig} from '../../config/api.config';

@Component({
	selector: 'app-two-factor',
	standalone: false,
	templateUrl: './two-factor.component.html',
	styleUrls: ['./two-factor.component.css']
})
export class TwoFactorComponent implements OnInit {
	twoFactorForm: FormGroup;
	email: string | null = null; // Store the email from query params

	constructor(private fb: FormBuilder, private route: ActivatedRoute, private http: HttpClient) {
		this.twoFactorForm = this.fb.group({
			digit1: ['', [Validators.required, Validators.pattern('^[0-9]$')]],
			digit2: ['', [Validators.required, Validators.pattern('^[0-9]$')]],
			digit3: ['', [Validators.required, Validators.pattern('^[0-9]$')]],
			digit4: ['', [Validators.required, Validators.pattern('^[0-9]$')]],
		});
	}

	ngOnInit() {
		// Fetch the 'email' query parameter from the URL
		this.route.queryParams.subscribe(params => {
			this.email = params['email'] || null; // Assign email from query params
			console.log('Email from query params:', this.email); // Optional: log the email
		});
	}

	onSubmit() {
		if (this.twoFactorForm.valid) {
			const code = `${this.twoFactorForm.value.digit1}${this.twoFactorForm.value.digit2}${this.twoFactorForm.value.digit3}${this.twoFactorForm.value.digit4}`;
			console.log('Two-factor code entered:', code);
			console.log('Email used for verification:', this.email);

			this.http.post(`${ApiConfig.apiUrl}/User/login_2FA?email=${this.email}&code=${code}`, null, {responseType: 'text'})
				.subscribe(r => console.log(r))
		}
	}

	onClear() {
		this.twoFactorForm.reset();
	}

	onClose() {
		console.log('Two-factor form closed');
		// Handle the close action here (e.g., navigate back or hide the form)
	}
}
