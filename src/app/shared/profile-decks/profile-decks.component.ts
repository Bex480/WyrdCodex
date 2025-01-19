import { Component } from '@angular/core';
import {Card} from '../../models/card.model';
import {CardService} from '../../services/card.service';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {ApiConfig} from '../../config/api.config';
import {getXHRResponse} from 'rxjs/internal/ajax/getXHRResponse';
import {Deck} from '../../models/deck.model';

@Component({
	selector: 'app-profile-decks',
	standalone: false,

	templateUrl: './profile-decks.component.html',
	styleUrl: './profile-decks.component.css'
})
export class ProfileDecksComponent {
	loading: boolean = true;
	decks: Deck[] = [];
	selectedDeck?: Deck;
	Details: boolean = true;

	constructor(private cardService: CardService, private http: HttpClient) {}

	ngOnInit(): void {

		const authToken = localStorage.getItem('authToken');

		const headers = new HttpHeaders({
			'Authorization': `Bearer ${authToken}`
		});

		this.http.get<Deck[]>(`${ApiConfig.apiUrl}/Card/decks`, { headers }).subscribe((response: Deck[]) => {
			this.decks = response;
		}, error => {
			console.error('Error:', error);
		});
	}

	ngOnChanges(){

	}

	showDetails(deck: Deck) {
		this.selectedDeck = deck;
	}
}
