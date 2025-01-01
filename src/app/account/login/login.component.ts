import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: false,

  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
	registerForm: FormGroup;
	isFormSubmitted: boolean = false;
	confirmPassword: any;

	constructor(private fb: FormBuilder, private http: HttpClient) {
		this.registerForm = this.fb.group({
			email: ['', [Validators.required]],
			password: ['', [Validators.required]]
		})
	}

	onSubmit() {
		this.isFormSubmitted = true;

		//DIDN'T KNOW IF I WAS STILL SUPPOSED TO USE SWAGGER FOR LOGIN/REGISTER
		/*

		 */
	}

	isInvalid(controlName: string): boolean {
		const control = this.registerForm.get(controlName);
		return !!(control && this.isFormSubmitted && !control.valid);
	}

	get email(){
		return this.registerForm.get('email');
	}

	get password(){
		return this.registerForm.get('password');
	}
}
