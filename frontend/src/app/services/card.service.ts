import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Card } from '../models/card.model';
import { ApiConfig } from '../config/api.config';

@Injectable({
	providedIn: 'root'
})
export class CardService {

	constructor(private http: HttpClient) {}

	getCards(filters?: any): Observable<Card[]> {
		let params = new HttpParams();

		if (filters) {
			if (filters.cardName) params = params.append('cardName', filters.cardName);
			if (filters.cardType) params = params.append('Type', filters.cardType);
			if (filters.cardFaction) params = params.append('Faction', filters.cardFaction);
		}

		return this.http.get<Card[]>(`${ApiConfig.apiUrl}/Card/shop`, { params });
	}

	getCardById(id: number): Observable<Card> {
		return this.http.get<Card>(`${ApiConfig.apiUrl}/Card?cardID=${id}`);
	}
}
