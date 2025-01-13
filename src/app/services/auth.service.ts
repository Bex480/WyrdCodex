import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ApiConfig } from '../config/api.config';

@Injectable({
	providedIn: 'root'
})
export class AuthService {

	constructor(private http: HttpClient, private router: Router) {}

	RefreshToken(): Observable<any> {
		const accessToken = localStorage.getItem("authToken");
		const refreshToken = localStorage.getItem("refToken");

		if (!accessToken || !refreshToken) {
			console.error('Missing tokens for refresh process');
			this.router.navigate(['/account/login']);
			return of(null);
		}

		return this.http.post(`${ApiConfig.apiUrl}/User/refresh_token`, {
			accessToken,
			refreshToken
		}).pipe(
			map((response: any) => {
				if (response?.token) {
					localStorage.setItem('authToken', response.token);
					return response.token;
				} else {
					this.router.navigate(['/account/login']);
					return null;
				}
			}),
			catchError(err => {
				console.error('Error during refresh token request:', err);
				this.router.navigate(['/account/login']);
				return of(null);
			})
		);
	}

	logout() {
		localStorage.removeItem("authToken");
		localStorage.removeItem("refToken");
		this.router.navigate(['/account/login']);
	}
}
