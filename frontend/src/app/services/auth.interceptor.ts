import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

	private isRefreshing = false;
	private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

	constructor(private authService: AuthService) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		// Adding Authorization header if authToken exists
		const authToken = localStorage.getItem('authToken');
		if (authToken) {
			req = this.addToken(req, authToken);
		}

		return next.handle(req).pipe(
			catchError((error: HttpErrorResponse) => {
				console.log('Interceptor caught an error:', error);
				if (error.status === 401) {
					return this.handle401Error(req, next);
				}
				return throwError(error);
			})
		);
	}

	private addToken(request: HttpRequest<any>, token: string) {
		return request.clone({
			setHeaders: {
				Authorization: `Bearer ${token}`
			}
		});
	}

	private handle401Error(request: HttpRequest<any>, next: HttpHandler) {

		if (!this.isRefreshing) {
			this.isRefreshing = true;
			this.refreshTokenSubject.next(null);

			return this.authService.RefreshToken().pipe(
				switchMap((newToken: any) => {

					if (newToken) {
						this.refreshTokenSubject.next(newToken);
						this.isRefreshing = false;  // Token refresh finished

						// Retrying the original request with the new token
						return next.handle(this.addToken(request, newToken));
					} else {
						this.isRefreshing = false;

						this.authService.logout();
						return throwError('Token refresh failed');
					}
				}),
				catchError((err) => {
					this.isRefreshing = false;
					this.authService.logout();
					return throwError(err);
				})
			);
		} else {

			return this.refreshTokenSubject.pipe(
				filter(token => token != null),
				take(1),
				switchMap(jwt => {
					return next.handle(this.addToken(request, jwt));
				})
			);
		}
	}
}
