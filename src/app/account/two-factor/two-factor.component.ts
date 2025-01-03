import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ApiConfig } from '../../config/api.config';

@Component({
	selector: 'app-two-factor',
	standalone: false,
	templateUrl: './two-factor.component.html',
	styleUrls: ['./two-factor.component.css']
})
export class TwoFactorComponent implements OnInit {
	email: string | null = null;
	otp: string = '';
	config = {
		length: 4,
		inputStyles: {
			width: '50px',
			height: '50px'
		}
	};
	loading: boolean = false;

	constructor(private route: ActivatedRoute, private  router: Router, private http: HttpClient) {}

	ngOnInit() {
		this.route.queryParams.subscribe(params => {
			this.email = params['email'] || null;
			console.log('Email from query params:', this.email);
		});
	}

	onOtpChange(otp: string) {
		this.otp = otp || '';
	}

	onSubmit() {
		if (this.otp.length === 4) {
			this.loading = true;
			console.log('Two-factor code entered:', this.otp);
			console.log('Email used for verification:', this.email);

			this.http.post(`${ApiConfig.apiUrl}/User/login_2FA?email=${this.email}&code=${this.otp}`, null, { observe: "response"})
				.subscribe({
					next: (response: any) => {
						if (response.status === 200) {
							const token = response.body.token;
							localStorage.setItem('authToken', token);
							console.log('Token stored:', token);

							this.router.navigate(['/dashboard']);
						}
					},
					error: (error) => {
						console.error('Error during verification:', error);
						this.loading = false;
						this.router.navigate(['/account/two-factor'], { queryParams: { email: this.email } })
					}
				});
		} else {
			console.error('OTP is not complete.');
		}
	}
}
