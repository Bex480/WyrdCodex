import {Component, Injectable} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {ApiConfig} from '../../config/api.config';
import {Router} from '@angular/router';


@Component({
	selector: 'app-register',
	standalone: false,
	templateUrl: './register.component.html',
	styleUrl: './register.component.css'
})

@Injectable({providedIn: 'root'})

export class RegisterComponent {
	registerForm: FormGroup;
	isFormSubmitted: boolean = false;
	confirmPassword: any;

	constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
		this.registerForm = this.fb.group({
			userName: ['', [Validators.required]],
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required, Validators.minLength(6)]],
			confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
		})
	}

	onSubmit() {
		this.isFormSubmitted = true;

		if(this.isInvalid('password') || this.isInvalid('userName') || this.isInvalid('email') || !this.areSame()){
			console.log("what?!");
			return;
		}

		const formData = {
			email: this.registerForm.value.email,
			password: this.registerForm.value.password,
			userName: this.registerForm.value.userName
		};


		this.http.post(`${ApiConfig.apiUrl}/User/register`, formData, { observe: 'response' })
			.subscribe(
				(response: any) => {
					console.log(response);
					if (response.status === 204) {
						this.http.post(`${ApiConfig.apiUrl}/User/login`, formData, { observe: 'response' })
							.subscribe(
								(response: any) => {
									if (response.status === 202) {
										this.router.navigate(['/account/two-factor'], { queryParams: { email: formData.email } });
									} else if (response.status === 200) {
										const token = response.body.token;
										localStorage.setItem('authToken', token);
										console.log('Token stored:', token);

										this.router.navigate(['/dashboard']);
									}
								},
								(error) => {
									console.error('Login failed:', error);
									this.router.navigate(['/account/login']);
								}
							);
					}
				},
				(error) => {
					console.error('Login failed:', error);
					alert(error);
				}
			);
	}

	isInvalid(controlName: string): boolean {
		const control = this.registerForm.get(controlName);
		return !!(control && this.isFormSubmitted && !control.valid);
	}

	get userName(){
		return this.registerForm.get('userName');
	}

	get email(){
		return this.registerForm.get('email');
	}

	get password(){
		return this.registerForm.get('password');
	}

	assignValue(value: any){
		this.confirmPassword = value;
	}

	areSame(){
		return this.registerForm.get('password')?.value == this.confirmPassword;
	}
}
