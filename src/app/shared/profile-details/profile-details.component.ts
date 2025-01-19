import { Component } from '@angular/core';
import {Deck} from '../../models/deck.model';
import {ApiConfig} from '../../config/api.config';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Profile} from '../../models/profile.model';

@Component({
  selector: 'app-profile-details',
  standalone: false,

  templateUrl: './profile-details.component.html',
  styleUrl: './profile-details.component.css'
})
export class ProfileDetailsComponent {
	profile!: Profile;

	constructor(private http: HttpClient) {
	}

	ngOnInit(){
		const authToken = localStorage.getItem('authToken');
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${authToken}`
		});

		this.http.get<Profile>(`${ApiConfig.apiUrl}/User/profile`, { headers }).subscribe(response =>{
			this.profile = response;
		}, error =>{
			console.log(error);
		});
	}
}
