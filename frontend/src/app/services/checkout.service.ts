import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Card } from '../models/card.model';
import { ApiConfig } from '../config/api.config';
import {map} from 'rxjs/operators';

@Injectable({
	providedIn: 'root'
})
export class CheckoutService {

	constructor(private http: HttpClient) {}

	checkout(){
		return this.http.post(`${ApiConfig.apiUrl}/Cart/checkout`, null);
	}

	getCart(): Observable<{ card: Card, quantity: number }[]> {
		return this.http.get<{ card: Card, quantity: number }[]>(`${ApiConfig.apiUrl}/Cart`);
	}

	addToCart(cardID: number) {
		return this.http.post(`${ApiConfig.apiUrl}/Cart/card/add?cardID=${cardID}`, null);
	}

	removeFromCart(cardID: number) {
		return this.http.delete(`${ApiConfig.apiUrl}/Cart/card/remove?cardID=${cardID}`);
	}

	decreaseCardQuantityInCart(cardID: number) {
		return this.http.put(`${ApiConfig.apiUrl}/Cart/card/quantity/decrease?cardID=${cardID}`, null);
	}

	setCardQuantityInCart(cardID: number, quantity: number) {
		return this.http.put(`${ApiConfig.apiUrl}/Cart/card/quantity/set?cardID=${cardID}&quantity=${quantity}`, null);
	}

	addToSaveForLater(cardID: number) {
		return this.http.post(`${ApiConfig.apiUrl}/Cart/card/save_for_later/add?cardID=${cardID}`, null);
	}

	removeFromSaveForLater(cardID: number) {
		return this.http.delete(`${ApiConfig.apiUrl}/Cart/card/save_for_later/remove?cardID=${cardID}`);
	}

	getSavedForLater(): Observable<{ card: Card, quantity: number }[]> {
		return this.http.get<{ card: Card, quantity: number }[]>(`${ApiConfig.apiUrl}/Cart/saved_for_later`);
	}

}
