import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, map, switchMap } from 'rxjs/operators';
import {ApiConfig} from '../config/api.config';

@Injectable({ providedIn: 'root' })
export class CardNameValidator implements AsyncValidator {

	constructor(private http: HttpClient) {}

	validate(control: AbstractControl): Observable<ValidationErrors | null> {
		const cardName = control.value;
		if (!cardName) {
			return of(null);
		}

		return of(cardName).pipe(
			debounceTime(300),
			switchMap(name =>
				this.http.get<{ exists: boolean }>(`${ApiConfig.apiUrl}/Card/exists`, { params: { name } }).pipe(
					map(response => (response.exists ? { cardNameTaken: true } : null)),
					catchError(() => of(null))
				)
			)
		);
	}
}
