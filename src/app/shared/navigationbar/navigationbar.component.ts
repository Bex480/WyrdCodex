import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-navigationbar',
  standalone: false,

  templateUrl: './navigationbar.component.html',
  styleUrl: './navigationbar.component.css'
})
export class NavigationbarComponent {
  isDropdownVisible = false;
  loginForm: FormGroup;
  userInformation: any;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    })
	  this.checkToken();

  }

  toggleDropdown(){
    this.isDropdownVisible = !this.isDropdownVisible;
  }

  onSubmit() {
    for(let i = 0; i < this.userInformation.length; i++){
      if(this.email?.value == this.userInformation[i].email){
        console.log("Form is valid!", this.loginForm.value);
        this.http.post('https://localhost:7088/api/User/login', this.loginForm.value, {responseType: 'text'}).subscribe(
          res => {console.log(res)}
        );
      }
    }
  }

  checkToken(): boolean{
	if(localStorage.getItem('authToken')){
		return true;
	}
	return false;
  }

  logOut(){
	  localStorage.removeItem('authToken');
	  localStorage.removeItem('refreshToken');
  }

  get email(){
    return this.loginForm.get('email');
  }

  get password(){
    return this.loginForm.get('password');
  }
}
