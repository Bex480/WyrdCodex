import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Card } from '../models/card.model';
import { ApiConfig } from '../config/api.config';

@Injectable({
	providedIn: 'root'
})
export class CardService {

	constructor(private http: HttpClient) {}

	getCards(): Observable<Card[]> {
		return this.http.get<Card[]>(`${ApiConfig.apiUrl}/Card/shop`);
	}

	getCardById(id: number): Observable<Card> {
		return this.http.get<Card>(`${ApiConfig.apiUrl}/Card?cardID=${id}`);
	}
}
