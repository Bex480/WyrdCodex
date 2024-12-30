import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-main',
  standalone: false,

  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  isDropdownVisible = false;
  loginForm: FormGroup;
  userInformation: any;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    })

    this.http.get('https://localhost:7088/api/User').subscribe(x =>{
      console.log(x);
      this.userInformation = x;
    })
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

  get email(){
    return this.loginForm.get('email');
  }

  get password(){
    return this.loginForm.get('password');
  }
}

